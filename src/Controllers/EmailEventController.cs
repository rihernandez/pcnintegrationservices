using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pcnintegrationservices.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace pcnintegrationservices.Controllers
{
    [ApiController]
    [Route("api/email-events")]
    [Tags("Email Events")]
    public class EmailEventController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailEventController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtener eventos de email paginados",
            Description = "Obtiene una lista paginada de eventos de email, con un máximo de 20 eventos por página. Utiliza el parámetro 'page' para navegar entre las páginas de resultados."
        )]
        public async Task<IActionResult> GetEmailEvents(int page = 1)
        {
            const int pageSize = 20;
            var emailEvents = await _context.EmailsEvents
                .OrderBy(e => e.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(emailEvents);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Obtener un evento de email por ID",
            Description = "Recupera los detalles de un evento de email específico mediante su ID único."
        )]
        public async Task<IActionResult> GetEmailEvent(int id)
        {
            var emailEvent = await _context.EmailsEvents.FindAsync(id);
            if (emailEvent == null)
                return NotFound(new { message = "Evento no encontrado" });

            return Ok(emailEvent);
        }

        [HttpGet("{recipient}")]
        [SwaggerOperation(
            Summary = "Obtener eventos de email por destinatario",
            Description = "Recupera todos los eventos de email asociados a un destinatario específico basado en su dirección de correo electrónico."
        )]
        public async Task<IActionResult> GetEmailEventsByRecipient(string recipient)
        {
            if (string.IsNullOrEmpty(recipient))
                return BadRequest(new { message = "El parámetro 'recipient' es requerido" });

            var emailEvents = await _context.EmailsEvents
                .Where(e => e.Recipient == recipient)
                .OrderBy(e => e.Timestamp)
                .ToListAsync();

            if (!emailEvents.Any())
                return NotFound(new { message = "No se encontraron eventos para el destinatario especificado" });

            return Ok(emailEvents);
        }

        [HttpDelete]
        [SwaggerOperation(
            Summary = "Eliminar todos los eventos de email",
            Description = "Elimina todos los registros de eventos de email almacenados en la base de datos."
        )]
        public async Task<IActionResult> DeleteAllEmailEvents()
        {
            _context.EmailsEvents.RemoveRange(_context.EmailsEvents);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Todos los eventos han sido eliminados" });
        }

        [HttpDelete("delete-by-date")]
        [SwaggerOperation(
            Summary = "Eliminar eventos de email anteriores a una fecha específica",
            Description = "Elimina todos los eventos de email registrados antes de la fecha especificada en el parámetro 'date'."
        )]
        public async Task<IActionResult> DeleteEmailEventsByDate(DateTime date)
        {
            var eventsToDelete = await _context.EmailsEvents
                .Where(e => e.Timestamp < date)
                .ToListAsync();

            if (!eventsToDelete.Any())
                return NotFound(new { message = "No se encontraron eventos anteriores a la fecha especificada" });

            _context.EmailsEvents.RemoveRange(eventsToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Se han eliminado {eventsToDelete.Count} eventos anteriores a {date:yyyy-MM-dd}" });
        }
    }
}
