namespace AppAngular.DTOS
{
    public class CreateUserDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        // Por esto:
        public List<string> Roles { get; set; } = new();

        //public string UserName {  get; set; }

        // public string Message { get; set; }

    }
}
