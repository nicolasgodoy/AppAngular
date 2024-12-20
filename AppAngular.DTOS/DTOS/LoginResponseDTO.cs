namespace AppAngular.DTOS
{
    public class LoginResponseDTO
    {
        public String Access_token { get; set; }

        public Int32 Expires_in { get; set; }

        public String Token_type { get; set; }

        public String Refresh_token { get; set; }
    }
}
