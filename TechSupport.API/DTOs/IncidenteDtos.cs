using System.ComponentModel.DataAnnotations;
using TechSupport.API.Models;

namespace TechSupport.API.DTOs;

public class UsuarioSummaryDto
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

public class IncidenteCreateDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "La descripción debe tener entre 5 y 2000 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    public PrioridadIncidente Prioridad { get; set; } = PrioridadIncidente.Media;

    // En Sesión 1 (sin autenticación aún), se puede indicar el usuario o asignar al default
    public int? UsuarioId { get; set; }
}

public class IncidenteResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public UsuarioSummaryDto? UsuarioReporta { get; set; }
    public int? TecnicoId { get; set; }
    public UsuarioSummaryDto? TecnicoAsignado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaResolucion { get; set; }
}
