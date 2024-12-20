namespace AppAngular.Domain.Models
{
    public class AuthConfiguration
    {
        public string BaseAddress { get; set; }

        public string ConfirmEmailEndpoint { get; set; }

        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public int ExpirationMinutes { get; set; }
    }
}
