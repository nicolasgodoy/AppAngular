using AppAngular.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvioEmailController : Controller
    {

        private readonly IAspNetUserService _aspNetUserService;

        public EnvioEmailController(IAspNetUserService aspNetUserService) 
        {
            _aspNetUserService = aspNetUserService;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail(string email, string token, string subject, string message)
        {
            // Verificación de los parámetros
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Email o contraseña no proporcionados.");
            }

            var callbackUrl = $"https://localhost:7057/api/EnvioEmailcontroller/confirmEmail?email={email}&token={token}";
            // Preparar el cuerpo del correo
            var body = $@"
                 <a href=""{callbackUrl}"" 
                    style=""padding: 10px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;"">
                    Confirmar Email
                 </a>";

            await _aspNetUserService.SendEmailAsync(email, token, subject, message, body);

            return Ok("Correo enviado exitosamente");
        }
    }
}
