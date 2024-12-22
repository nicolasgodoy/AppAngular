namespace AppAngular.DTOS
{
    public class CrearUsuarioDTO
    {
        // ahora si dejando solucion subida y bien estructurada.
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public string UserName {  get; set; }

        public string Message { get; set; }

    }
}
