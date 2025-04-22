using AppAngular.Domain.Interfaces;
using AppAngular.DTOS;
using AppAngular.DTOS.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService _publicationService;

        public PublicationController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<PublicationDTO>>> GetAllUsers()
        {
            var publicationDto = await _publicationService.GetAllAsync();

            return Ok(publicationDto);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<CreatePublicationDTO>> CreateCategory([FromBody] CreatePublicationDTO publicationCreateDto)
        {
            await _publicationService.AddAsync(publicationCreateDto);

            return Ok(publicationCreateDto);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdatePublicationDTO>> UpdateCategory([FromBody] UpdatePublicationDTO publicationUpdateDto)
        {
            await _publicationService.UpdateAsync(publicationUpdateDto);

            return Ok(publicationUpdateDto);
        }
    }
}
