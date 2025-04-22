using AppAngular.Domain.Models;

namespace AppAngular.Domain.IRepository
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllAsync();
        Task<Publication> GetByIdAsync(int id);
        Task<Publication> AddAsync(Publication publicacion);
        Task<Publication> UpdateAsync(int id, Publication publicacion);
        Task DeleteAsync(int id);
    }
}
