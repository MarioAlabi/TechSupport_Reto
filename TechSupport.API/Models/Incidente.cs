namespace TechSupport.API.Models;

public class Incidente
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public PrioridadIncidente Prioridad { get; set; } = PrioridadIncidente.Media;
    public EstadoIncidente Estado { get; set; } = EstadoIncidente.Pendiente;

    // Relación 1: Usuario que reporta el incidente
    public int UsuarioId { get; set; }
    public Usuario? UsuarioReporta { get; set; }

    // Relación 2: Técnico asignado para atender el incidente (nulo inicialmente)
    public int? TecnicoId { get; set; }
    public Usuario? TecnicoAsignado { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaResolucion { get; set; }
}
