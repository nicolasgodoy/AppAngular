using AppAngular.Domain.Interfaces;
using AppAngular.DTOS.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categorynService)
        {
            _categoryService = categorynService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllUsers()
        {
            var categoryDto = await _categoryService.GetAllAsync();

            return Ok(categoryDto);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<CreateCategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO categoryCreate)
        {
            await _categoryService.AddAsync(categoryCreate);

            return Ok(categoryCreate);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UpdateCategoryDTO>> UpdateCategory([FromBody] UpdateCategoryDTO categoryUpdate)
        {
            await _categoryService.UpdateAsync(categoryUpdate);

            return Ok(categoryUpdate);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok();
        }
    }
}
