using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using pcnintegrationservices.Data;
using pcnintegrationservices.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace pcnintegrationservices.Controllers
{
    [ApiController]
    [Route("api/emails/webhook")]
    [Tags("Emails")]
    public class EmailWebhookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailWebhookController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("receive")]
        [SwaggerOperation(
            Summary = "Recibir y procesar eventos de email desde un webhook",
            Description = "Este endpoint recibe y procesa eventos de email enviados a través de un webhook, deserializa la carga JSON y, opcionalmente, guarda la información en la base de datos. Se registran los eventos recibidos en la consola para verificación."
        )]
        [HttpPost]
        public IActionResult ReceiveWebhook([FromBody] EmailsEvent data)
        {
            if (data == null)
            {
                return BadRequest(new { error = "El cuerpo de la solicitud no contiene datos válidos." });
            }

            // Registro de información recibida
            Console.WriteLine("Webhook recibido:");
            Console.WriteLine($"Evento: {data.Event}");
            Console.WriteLine($"Destinatario: {data.Recipient}");
            Console.WriteLine($"ID de Mensaje: {data.MessageId}");
            Console.WriteLine($"Timestamp: {data.Timestamp}");

            try
            {
                // Guardar el evento en la base de datos
                // data.Timestamp = DateTime.UtcNow; // Actualizar la hora de recepción
                // _context.EmailsEvents.Add(data);
                // await _context.SaveChangesAsync();

                return Ok(new { message = "Evento recibido con éxito" });
            }
            catch (Exception ex)
            {
                // Manejo de errores relacionados con la base de datos u otros
                return StatusCode(500, new { error = "Error al procesar el evento", details = ex.Message });
            }
        }

    }
}
