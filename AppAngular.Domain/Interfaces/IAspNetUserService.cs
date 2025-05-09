using AppAngular.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface IAspNetUserService
    {
        Task<string> GetUserByIdAsync(string id);
        Task<IEnumerable<AspNetUserDTO>> GetAllUsersAsync();

        Task<CreateUserDTO> CreateUserAsync(CreateUserDTO userDto);

        Task<bool> ConfirmEmailAsync(string userId, string code);
    }

}