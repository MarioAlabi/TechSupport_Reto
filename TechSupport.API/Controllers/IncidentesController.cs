using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechSupport.API.Data;
using TechSupport.API.DTOs;
using TechSupport.API.Models;

namespace TechSupport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<IncidentesController> _logger;

    public IncidentesController(AppDbContext context, ILogger<IncidentesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Consulta todos los incidentes registrados en el sistema.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IncidenteResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IncidenteResponseDto>>> GetAll()
    {
        var incidentes = await _context.Incidentes
            .Include(i => i.UsuarioReporta)
            .Include(i => i.TecnicoAsignado)
            .OrderByDescending(i => i.FechaCreacion)
            .ToListAsync();

        return Ok(incidentes.Select(MapToDto));
    }

    /// <summary>
    /// Obtiene el detalle de un incidente por su identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IncidenteResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IncidenteResponseDto>> GetById(int id)
    {
        var incidente = await _context.Incidentes
            .Include(i => i.UsuarioReporta)
            .Include(i => i.TecnicoAsignado)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (incidente == null)
        {
            return NotFound(new { message = $"No se encontró el incidente con ID {id}." });
        }

        return Ok(MapToDto(incidente));
    }

    /// <summary>
    /// Crea un nuevo incidente con estado inicial Pendiente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(IncidenteResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IncidenteResponseDto>> Create([FromBody] IncidenteCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Asociar al usuario reportador (o al primer usuario disponible para la Sesión 1)
        int reporterId = dto.UsuarioId ?? 0;
        if (reporterId <= 0 || !await _context.Usuarios.AnyAsync(u => u.Id == reporterId))
        {
            var defaultUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Rol == RolUsuario.Usuario)
                              ?? await _context.Usuarios.FirstOrDefaultAsync();

            if (defaultUser == null)
            {
                return BadRequest(new { message = "No hay usuarios registrados en el sistema para asociar el incidente." });
            }
            reporterId = defaultUser.Id;
        }

        var incidente = new Incidente
        {
            Titulo = dto.Titulo.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            Prioridad = dto.Prioridad,
            Estado = EstadoIncidente.Pendiente,
            UsuarioId = reporterId,
            FechaCreacion = DateTime.UtcNow
        };

        _context.Incidentes.Add(incidente);
        await _context.SaveChangesAsync();

        // Cargar entidad relacionada para la respuesta
        await _context.Entry(incidente).Reference(i => i.UsuarioReporta).LoadAsync();

        var responseDto = MapToDto(incidente);
        return CreatedAtAction(nameof(GetById), new { id = incidente.Id }, responseDto);
    }

    private static IncidenteResponseDto MapToDto(Incidente i)
    {
        return new IncidenteResponseDto
        {
            Id = i.Id,
            Titulo = i.Titulo,
            Descripcion = i.Descripcion,
            Prioridad = i.Prioridad.ToString(),
            Estado = i.Estado.ToString(),
            UsuarioId = i.UsuarioId,
            UsuarioReporta = i.UsuarioReporta != null ? new UsuarioSummaryDto
            {
                Id = i.UsuarioReporta.Id,
                NombreUsuario = i.UsuarioReporta.NombreUsuario,
                Rol = i.UsuarioReporta.Rol.ToString()
            } : null,
            TecnicoId = i.TecnicoId,
            TecnicoAsignado = i.TecnicoAsignado != null ? new UsuarioSummaryDto
            {
                Id = i.TecnicoAsignado.Id,
                NombreUsuario = i.TecnicoAsignado.NombreUsuario,
                Rol = i.TecnicoAsignado.Rol.ToString()
            } : null,
            FechaCreacion = i.FechaCreacion,
            FechaResolucion = i.FechaResolucion
        };
    }
}
