using AppAngular.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppAngular
{
    public class AplicationDbContext : IdentityDbContext<AspNetUsers>
    {
        public DbSet<AspNetUsers> AspNetUsers { get; set; }

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configura cualquier mapeo adicional si es necesario
        }
    }
}
