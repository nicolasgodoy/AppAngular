using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using AppAngular.DTOS;
using AppAngular.Service.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
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

        public async Task<CrearUsuarioDTO> CreateUserAsync(CrearUsuarioDTO userDto)
        {
            var emailStore = (IUserEmailStore<AspNetUsers>)_userStore;

            var user = new AspNetUsers();
            user.RefreshToken = Guid.NewGuid().ToString();

            await _userStore.SetUserNameAsync(user, userDto.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, userDto.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, userDto.PasswordHash);
            if (!result.Succeeded)
            {
                // Crear un mensaje de error detallado
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                var errorMessage = $"Error al crear el usuario: {errorMessages}";

                // Devolver null para indicar un fallo
                Console.WriteLine(errorMessage); // Opcional: Log del error
                return null;
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
            var response = new CrearUsuarioDTO
            {
                Email = user.Email,
                UserName = user.UserName,
                Message = "Confirm Your Email Address to Complete Your Registration"
            };
      
            return response;
        }

        // EL QUE HABIA ECHO YO
        //public async Task<CrearUsuarioDTO> CreateUserAsync2(CrearUsuarioDTO userDto)
        //{
        //    // Validar si el correo electrónico ya está confirmado
        //    if (!userDto.EmailConfirmed)
        //    {
        //        // para futuro manejo de errores por ahora siempre lo ponemos en true
        //        throw new ArgumentException("El correo electrónico no está confirmado.");
        //    }
        //    // Validar si el nombre de usuario ya existe
        //    var existingUser = await _repository.GetAllAsync();
        //    if (existingUser.Any(u => u.UserName == userDto.UserName))
        //    {
        //        // Si el nombre de usuario ya existe, lanza una excepcion
        //        throw new ArgumentException("El nombre de usuario ya está en uso.");
        //    }
        //    // Mapeo del DTO a la entidad `AspNetUsers`
        //    var userEntity = _mapper.Map<AspNetUsers>(userDto);

        //    await _repository.AddAsync(userEntity);

        //    // Enviar correo de bienvenida
        //    var subject = "Bienvenido a nuestra aplicación";
        //    var body = $"Hola {userDto.UserName}, gracias por registrarte.";
        //    await SendEmailAsync(userDto.Email, userDto.PasswordHash, userDto.UserName, subject, body);

        //    // Devolver el usuario creado (envolver en un Task)
        //    return await Task.FromResult(userDto);
        //}

        public async Task<IEnumerable<AspNetUserDTO>> GetAllUsersAsync()
        {

            var users = await _repository.GetAllAsync();

            return users.Select(user => new AspNetUserDTO
            {
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
     
            });

        }

        public async Task SendEmailAsync(string email, string token, string subject, string message, string body)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Cambia el puerto según tu proveedor
                    Credentials = new NetworkCredential(email, token),
                    EnableSsl = true,
                })
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(email),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(email);  // Cambiar aquí si deseas otro destinatario

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                // Log o manejar el error
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                throw; // O devolver un error personalizado
            }
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
