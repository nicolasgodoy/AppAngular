using System.ComponentModel.DataAnnotations;

namespace AppAngular.Domain.Models
{
    public class Register
    {
        [EmailAddress]
        public required string Email { get; init; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; init; }
    }
}
