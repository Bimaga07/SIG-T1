
using SIGT.Domain.Entities;

namespace SIGT.Domain.Interfaces;

public interface ITareaRepository
{
    Task<IEnumerable<dynamic>> GetAllWithUserAsync(); // Para la Vista
    Task CreateAsync(Tarea tarea); // Para el SP Create
    Task UpdateAsync(Tarea tarea); // Para el SP Update
    Task DeleteAsync(int id);
}