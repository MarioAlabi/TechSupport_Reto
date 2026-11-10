using BCrypt.Net;
using TechSupport.API.Models;

namespace TechSupport.API.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context, ILogger logger)
    {
        // Asegura la existencia del archivo SQLite y las tablas
        context.Database.EnsureCreated();

        if (context.Usuarios.Any())
        {
            logger.LogInformation("Base de datos ya cuenta con usuarios inicializados.");
            return;
        }

        logger.LogInformation("Sembrando usuarios de prueba con contraseñas hasheadas (BCrypt)...");

        var usuarios = new List<Usuario>
        {
            new()
            {
                NombreUsuario = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Rol = RolUsuario.Administrador
            },
            new()
            {
                NombreUsuario = "tecnico1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tecnico123!"),
                Rol = RolUsuario.Tecnico
            },
            new()
            {
                NombreUsuario = "tecnico2",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Tecnico123!"),
                Rol = RolUsuario.Tecnico
            },
            new()
            {
                NombreUsuario = "usuario1",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Usuario123!"),
                Rol = RolUsuario.Usuario
            },
            new()
            {
                NombreUsuario = "usuario2",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Usuario123!"),
                Rol = RolUsuario.Usuario
            }
        };

        context.Usuarios.AddRange(usuarios);
        context.SaveChanges();

        logger.LogInformation("Sembrado completado: {Count} usuarios creados con contraseñas seguras.", usuarios.Count);
    }
}
