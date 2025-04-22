using AppAngular.Domain.Interfaces;
using AppAngular.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticacionController : Controller
    {
        private readonly IAuthenticacionService _authenticacion;
        private readonly IAspNetUserService _aspNetUserService;
        public AuthenticacionController(IAuthenticacionService authenticacion, IAspNetUserService aspNetUserService) 
        { 
            _authenticacion = authenticacion;
            _aspNetUserService = aspNetUserService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<IEnumerable<CreateUserDTO>>> Create([FromBody] CreateUserDTO userDto)
        {
            await _aspNetUserService.CreateUserAsync(userDto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<IEnumerable<LoginResponseDTO>>> Login(LoginDTO loginDto)
        {

            var result = await _authenticacion.SignInAsync(loginDto);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Login(RefreshDTO refreshDto)
        {
            var response = await _authenticacion.RefreshToken(refreshDto);

            return Ok(response);  
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string UserId, string Code)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                return BadRequest("Email o token no proporcionados.");
            }

            var result = await _aspNetUserService.ConfirmEmailAsync(UserId, Code);

            return result ? Ok() : BadRequest();
        }
    }
}
