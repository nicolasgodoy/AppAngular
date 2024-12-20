using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAngular.Domain.Models
{
    // HAY QUE LLEVARLO A LA CAPA CORE PERO NOSE PORQUE NO ME LA TOMA

    [Table("AspNetUsers")]
    public class AspNetUsers : IdentityUser
    { 
        public string RefreshToken { get; set; }

    }
}
