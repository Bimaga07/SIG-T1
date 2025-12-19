
using SIGT.Domain.Entities;
using SIGT.Domain.Interfaces;
using SIGT.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias: Conectamos la Interfaz con el Repositorio de SQL
builder.Services.AddScoped<ITareaRepository, TareaRepository>();

// Configuración de CORS (Importante para que Blazor pueda conectarse)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. CONFIGURACIÓN DEL MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(); // Activamos CORS

// 3. ENDPOINTS (MINIMAL API)

// GET: Obtener todas las tareas (Usa la Vista de SQL)
app.MapGet("/api/tareas", async (ITareaRepository repo) =>
{
    var tareas = await repo.GetAllWithUserAsync();
    return Results.Ok(tareas);
});

// POST: Crear una nueva tarea (Usa el SP de SQL)
app.MapPost("/api/tareas", async (ITareaRepository repo, Tarea tarea) =>
{
    await repo.CreateAsync(tarea);
    return Results.Created($"/api/tareas/{tarea.Id}", tarea);
});

// PUT: Actualizar una tarea (Usa el SP de SQL)
app.MapPut("/api/tareas", async (ITareaRepository repo, Tarea tarea) =>
{
    await repo.UpdateAsync(tarea);
    return Results.NoContent();
});

// DELETE: Eliminar una tarea
app.MapDelete("/api/tareas/{id:int}", async (ITareaRepository repo, int id) =>
{
    await repo.DeleteAsync(id);
    return Results.NoContent();
});

// REQUISITO MÓDULO 9: Endpoint de tarea lenta (Retorna 202 Accepted)
app.MapPost("/api/reporte/tareas-finalizadas", () =>
{
    // Este código simula que la petición fue recibida y se procesará después
    return Results.Accepted();
});

app.Run();