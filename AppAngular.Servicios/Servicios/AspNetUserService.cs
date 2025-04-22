using AppAngular.Domain.Interfaces;
using AppAngular.Domain.IRepository;
using AppAngular.Domain.Models;
using AppAngular.DTOS;
using AppAngular.Service.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;


namespace AppAngular.Servicios
{
    public class AspNetUserService : IAspNetUserService
    {
        private readonly IOptions<AuthConfiguration> _authConfiguration;
        private readonly IRepository<AspNetUsers> _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly IUserStore<AspNetUsers> _userStore;
        private readonly IEnviarEmailService _enviarEmailService;
        public AspNetUserService(
            IRepository<AspNetUsers> repository,  
            IMapper mapper, 
            IUserStore<AspNetUsers> userStore, 
            UserManager<AspNetUsers> userManager,
            IEnviarEmailService enviarEmailService,
            IOptions<AuthConfiguration> authConfiguration)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _userManager = userManager;
            _userStore = userStore;
            _enviarEmailService = enviarEmailService;
            _authConfiguration = authConfiguration;
        }

        public async Task<CreateUserDTO> CreateUserAsync(CreateUserDTO userDto)
        {
            var emailStore = (IUserEmailStore<AspNetUsers>)_userStore;

            var user = new AspNetUsers();
            user.RefreshToken = Guid.NewGuid().ToString();

            await _userStore.SetUserNameAsync(user, userDto.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, userDto.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                // Crear un mensaje de error detallado
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                var errorMessage = $"Error al crear el usuario: {errorMessages}";

                // Devolver null para indicar un fallo
                throw new Exception(errorMessage); // Opcional: Log del error
    
            }

            var authConfiguration = _authConfiguration.Value; // Accediendo a la configuración

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            if (string.IsNullOrEmpty(authConfiguration.BaseAddress))
            {
                throw new InvalidOperationException("BaseAddress no está configurado correctamente.");
            }

            var baseUri = new Uri(authConfiguration.BaseAddress);
            var uriBuilder = new UriBuilder(baseUri)
            {
                Path = authConfiguration.ConfirmEmailEndpoint,
            };

            Dictionary<string, string> queryParams = new() { { "UserId", user.Id.ToString() }, { "Code", code } };

            var confirmEmailUrl = QueryHelpers.AddQueryString(uriBuilder.Uri.ToString(), queryParams);

            //mañana ver bien como tiene que quedar este path
            string content = Assembly.Load("AppAngular").GetResourceAsString("AppAngular.Identity.Register.ConfirmationEmail.html");

            content = content.Replace("{{UserName}}", user.Email)
                             .Replace("{{ConfirmationLink}}", HtmlEncoder.Default.Encode(confirmEmailUrl));

            await _enviarEmailService.SendEmailAsync(user.Email, "Confirm Your Email Address to Complete Your Registration", content);

            // Construir y devolver el DTO
            var response = new CreateUserDTO
            {
                Email = user.Email
            };
      
            return response;
        }

        public async Task<IEnumerable<AspNetUserDTO>> GetAllUsersAsync()
        {

            var users = await _repository.GetAllAsync();

            return users.Select(user => new AspNetUserDTO
            {
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Emailconfirmed = user.EmailConfirmed
     
            });

        }

        public async Task<bool> ConfirmEmailAsync(string UserId, string Code)
        {
            // Buscar al usuario por el correo electrónico
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));

            // Confirmar el correo electrónico
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new Exception("Error al confirmar el correo electrónico.");
            }

            return true; // Confirmación exitosa
        }


        public Task<AspNetUserDTO> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }


        public async Task DeleteUserAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }


        public Task UpdateUserAsync(AspNetUserDTO user)
        {
            throw new NotImplementedException();
        }

        

        #region Metodos Privados
        #endregion
    }
}
