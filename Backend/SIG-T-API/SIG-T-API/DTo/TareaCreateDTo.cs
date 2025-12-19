

namespace SIGT.Domain.DTo;

public class TareaCreateDTo
{
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
}