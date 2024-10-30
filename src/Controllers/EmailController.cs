using Microsoft.AspNetCore.Mvc;
using pcnintegrationservices.Services;
using pcnintegrationservices.Models; 
using Swashbuckle.AspNetCore.Annotations;

namespace pcnintegrationservices.Controllers
{
    [ApiController]
    [Route("api/emails")]
    [Tags("Emails")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        [SwaggerOperation(
            Summary = "Enviar un correo electrónico",
            Description = "Envía un correo electrónico a la dirección especificada con el asunto y el contenido proporcionado en el cuerpo. Requiere los campos 'To', 'Subject' y 'Body'. Devuelve una confirmación si el correo fue enviado exitosamente o un error si ocurrió algún problema en el proceso."
        )]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (string.IsNullOrEmpty(emailRequest.To) || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Body))
            {
                return BadRequest("Faltan datos necesarios para enviar el correo.");
            }

            var result = await _emailService.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);

            if (result)
                return Ok("Correo enviado exitosamente.");
            
            return StatusCode(500, "Hubo un problema al enviar el correo.");
        }
    }
}
