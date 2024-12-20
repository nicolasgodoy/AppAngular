using AppAngular.DTOS;

namespace AppAngular.Domain.Interfaces
{
    public interface IAuthenticacionService
    {
        Task<string> GetUserByEmailAsync(string email);

        Task<bool> CheckPasswordAsync(LoginDTO user);

        Task<LoginResponseDTO> SignInAsync(LoginDTO email);

        Task<string> GenerateEmailConfirmationTokenAsync(string userId);

        Task<RefreshResponseDTO> RefreshToken(RefreshDTO refreshDto);

    }
}
