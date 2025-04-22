using AppAngular.Domain.Models;

namespace AppAngular.Domain.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(int id, Category category);
        Task DeleteAsync(int id);
    }
}
