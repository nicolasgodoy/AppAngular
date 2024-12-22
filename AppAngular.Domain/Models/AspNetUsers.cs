using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAngular.Domain.Models
{

    [Table("AspNetUsers")]
    public class AspNetUsers : IdentityUser
    { 
        public string RefreshToken { get; set; }

    }
}
