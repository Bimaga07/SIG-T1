using SIGT.Domain.Entities;
using SIGT.Domain.Interfaces;
using SIGT.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();

// IMPORTANTE: Registrar Autorización para que app.UseAuthorization() no de error
builder.Services.AddAuthorization();

// CONFIGURACIÓN DE CORS: Permite que el Frontend entre a la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. CONFIGURACIÓN DEL MIDDLEWARE (EL ORDEN ES VITAL)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// PRIMERO CORS, LUEGO AUTORIZACIÓN
app.UseCors("AllowAll");
app.UseAuthorization();

// 3. ENDPOINTS (MINIMAL API)

// GET: Obtener todas las tareas
app.MapGet("/api/tareas", async (ITareaRepository repo) =>
{
    var tareas = await repo.GetAllWithUserAsync();
    return Results.Ok(tareas);
});

// POST: Crear una nueva tarea
app.MapPost("/api/tareas", async (ITareaRepository repo, Tarea tarea) =>
{
    await repo.CreateAsync(tarea);
    return Results.Created($"/api/tareas/{tarea.Id}", tarea);
});

// PUT: Actualizar una tarea
app.MapPut("/api/tareas/{id:int}", async (int id, Tarea tarea, ITareaRepository repo) =>
{
    tarea.Id = id; // Aseguramos que el objeto tenga el ID de la URL
    await repo.UpdateAsync(tarea);
    return Results.Ok("Tarea actualizada con éxito");
});

// DELETE: Eliminar una tarea
app.MapDelete("/api/tareas/{id:int}", async (int id, ITareaRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.Ok($"Tarea {id} eliminada");
});

// REQUISITO MÓDULO 9: Endpoint de reporte (HTTP 202)
app.MapPost("/api/reporte/tareas-finalizadas", () =>
{
    return Results.Accepted();
});

app.Run();