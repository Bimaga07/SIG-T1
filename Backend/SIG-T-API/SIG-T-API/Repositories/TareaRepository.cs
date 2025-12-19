using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SIGT.Domain.Entities;
using SIGT.Domain.Interfaces;
using System.Data;

namespace SIGT.Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly string _connectionString;

    public TareaRepository(IConfiguration config)
    {
        // Se obtiene la cadena desde appsettings.json
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<dynamic>> GetAllWithUserAsync()
    {
        var list = new List<dynamic>();
        using var conn = new SqlConnection(_connectionString);
        // Requisito: Uso de la Vista vw_TareasConUsuario
        using var cmd = new SqlCommand("SELECT * FROM vw_TareasConUsuario", conn);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new
            {
                Id = reader["ID"],
                Titulo = reader["Titulo"],
                Estado = reader["Estado"],
                UsuarioAsignado = reader["UsuarioAsignado"]
            });
        }
        return list;
    }

    public async Task CreateAsync(Tarea tarea)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_Tarea_Create", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        // Parámetros obligatorios para el SP y seguridad contra SQL Injection
        cmd.Parameters.AddWithValue("@Titulo", tarea.Titulo);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)tarea.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", tarea.Estado);
        cmd.Parameters.AddWithValue("@UsuarioID", tarea.UsuarioId);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Tarea tarea)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_Tarea_Update", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        // Requisito: Los UPDATE deben ser por Procedimientos Almacenados
        cmd.Parameters.AddWithValue("@ID", tarea.Id);
        cmd.Parameters.AddWithValue("@Titulo", tarea.Titulo);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)tarea.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", tarea.Estado);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        // Para el Delete, la rúbrica no exige SP, así que usamos query directo parametrizado
        using var cmd = new SqlCommand("DELETE FROM Tareas WHERE ID = @ID", conn);
        cmd.Parameters.AddWithValue("@ID", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}