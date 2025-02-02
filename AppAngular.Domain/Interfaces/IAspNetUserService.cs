using AppAngular.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface IAspNetUserService
    {
        Task<IEnumerable<AspNetUserDTO>> GetAllUsersAsync();

        Task<CrearUsuarioDTO> CreateUserAsync(CrearUsuarioDTO userDto);

        //Task SendEmailAsync(string email,string token, string subject, string message, string body);

        Task<bool> ConfirmEmailAsync(string userId, string code);
    }

}