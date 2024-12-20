using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using AppAngular.DTOS;
using AppAngular.Service.Servicios;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace AppAngular.Authenticacion
{

    public class AuthenticacionService : IAuthenticacionService
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly AuthConfiguration _authConfiguration;
        private readonly JwtService _jwtService;

        public AuthenticacionService(
                UserManager<AspNetUsers> userManager, 
                SignInManager<AspNetUsers> signInManager, 
                AuthConfiguration authConfiguration,
                JwtService jwtService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authConfiguration = authConfiguration;
            _jwtService = jwtService;
        }

        public async Task<string> GetUserByEmailAsync(string email)
        {
            // Validar formato del email
            if (!IsValidEmail(email))
            {
                throw new ArgumentException("El formato del email no es válido.");
            }
            // Buscar usuario por email
            try 
            {
                await _userManager.FindByEmailAsync(email);

                if (email == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }
                return email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckPasswordAsync(LoginDTO loginDto)
        {

            //IsBase64String(loginDto.Password);

            // Validar que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                throw new ArgumentException("La contraseña no puede estar vacía.");
            }

            var user = new AspNetUsers
            {
                Email = loginDto.Email,
                PasswordHash = loginDto.Password
            };

            // Verificar contraseña
            await _userManager.CheckPasswordAsync(user, user.PasswordHash);
            if (loginDto.Password == null)
            {
                throw new Exception("Contraseña incorrecta.");
            }

            return true;
        }

        public async Task<LoginResponseDTO> SignInAsync(LoginDTO loginDto)
        {
            // Validar email y obtener usuario
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            // Validar contraseña
            //await CheckPasswordAsync(loginDto);

            // Realizar inicio de sesión
            await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,  false, false);
            if (loginDto.Email == null || loginDto.Password == null)
            {
                throw new Exception("Inicio de sesión fallido.");
            }

            string acessToken = await _jwtService.GenerateAccessToken(user);

            LoginResponseDTO response = new()
            {
                Access_token = acessToken,
                Refresh_token = user.RefreshToken,
                Token_type = "Bearer",
                Expires_in = Convert.ToInt32(TimeSpan.FromMinutes(_authConfiguration.ExpirationMinutes).TotalSeconds)
            };

            return response;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            // Buscar al usuario en la base de datos
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("El usuario no existe.");

            // Generar token de confirmación de email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return token;
        }

        public async Task<RefreshResponseDTO> RefreshToken(RefreshDTO refreshDto)
        {
            _jwtService.GetPrincipalFromToken(refreshDto.Access_token);

            var principal = _jwtService.GetPrincipalFromToken(refreshDto.Access_token, validateExpiration: false);
            var username = principal.Identity.Name;

            var user = await _userManager.FindByEmailAsync(username);

            // Verifica si el RefreshToken no coincide.
            if (user.RefreshToken != refreshDto.Refresh_token)
                throw new UnauthorizedAccessException("Invalid refreshToken");

            string acessToken = await _jwtService.GenerateAccessToken(user);

            RefreshResponseDTO response = new()
            {
                Access_token = acessToken,
                Refresh_token = user.RefreshToken,
                Token_type = "Bearer",
                Expires_in = Convert.ToInt32(TimeSpan.FromMinutes(_authConfiguration.ExpirationMinutes).TotalSeconds)
            };

            return response;
        }



        #region Metodos Privados

        private bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato del email
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private static bool IsBase64String(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                // Intenta convertir el string
                Convert.FromBase64String(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        #endregion
    }
}
