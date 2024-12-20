namespace AppAngular.DTOS
{
    public class CrearUsuarioDTO
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName {  get; set; }

        public string Message { get; set; }

    }
}
