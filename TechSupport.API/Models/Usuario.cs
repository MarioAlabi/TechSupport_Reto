namespace TechSupport.API.Models;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }

    // Propiedades de navegación
    public ICollection<Incidente> IncidentesReportados { get; set; } = new List<Incidente>();
    public ICollection<Incidente> IncidentesAsignados { get; set; } = new List<Incidente>();
}
