using AppAngular.Domain.Interfaces;
using AppAngular.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


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

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserDTO>>> GetAllUsers()
        {
            var usersDto = await _aspNetUserService.GetAllUsersAsync();

            return Ok(usersDto);
        }

    }
}
