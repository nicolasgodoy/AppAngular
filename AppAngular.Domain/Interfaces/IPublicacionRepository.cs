using AppAngular.Domain.Models;

namespace AppAngular.Domain.Interfaces
{
    public interface IPublicacionRepository
    {
        Task<IEnumerable<Publicacion>> GetAllAsync();
        Task<Publicacion> GetByIdAsync(int id);
        Task<Publicacion> AddAsync(Publicacion publicacion);
        Task<Publicacion> UpdateAsync(int id, Publicacion publicacion);
        Task DeleteAsync(int id);
    }
}
