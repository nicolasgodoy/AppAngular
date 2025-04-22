using AppAngular.DTOS.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task AddAsync(CreateCategoryDTO categoryCreate);
        Task UpdateAsync(UpdateCategoryDTO categoryUpdate);
        Task DeleteAsync(int id);
    }
}
