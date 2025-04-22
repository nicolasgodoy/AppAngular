using AppAngular.DTOS.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface IPublicationService
    {
        Task<IEnumerable<PublicationDTO>> GetAllAsync();
        Task<PublicationDTO> GetByIdAsync(int id);
        Task AddAsync(CreatePublicationDTO publicacion);
        Task UpdateAsync(UpdatePublicationDTO publicacion);
        Task DeleteAsync(int id);
    }
}
