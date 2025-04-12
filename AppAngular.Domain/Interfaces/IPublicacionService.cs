using AppAngular.Domain.Models;
using AppAngular.DTOS.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface IPublicacionService
    {
        Task<IEnumerable<PublicacionDTO>> GetAllAsync();
        Task<PublicacionDTO> GetByIdAsync(int id);
        Task AddAsync(PublicacionDTO publicacion);
        Task UpdateAsync(PublicacionDTO publicacion);
        Task DeleteAsync(int id);
    }
}
