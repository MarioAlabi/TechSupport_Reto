namespace TechSupport.API.Models;

public enum RolUsuario
{
    Administrador,
    Tecnico,
    Usuario
}

public enum EstadoIncidente
{
    Pendiente,
    Asignado,
    EnProceso,
    Resuelto,
    Cerrado
}

public enum PrioridadIncidente
{
    Baja,
    Media,
    Alta,
    Critica
}
