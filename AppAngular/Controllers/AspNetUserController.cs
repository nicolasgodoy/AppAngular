using AppAngular.Domain.Interfaces;
using AppAngular.DTOS;
using Microsoft.AspNetCore.Mvc;


namespace AppAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AspNetUsersController : ControllerBase
    {
        private readonly IAspNetUserService _aspNetUserService;

        public AspNetUsersController(IAspNetUserService userService)
        {
            _aspNetUserService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserDTO>>> GetAllUsers()
        {
            var usersDto = await _aspNetUserService.GetAllUsersAsync();

            return Ok(usersDto);
        }

    }
}
