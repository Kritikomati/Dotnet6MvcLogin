using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dotnet6MvcLogin.Models.DTO;

namespace Dotnet6MvcLogin.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Dotnet6MvcLogin.Models.DTO.RegistrationModel>? RegistrationModel { get; set; }
        public DbSet<Dotnet6MvcLogin.Models.DTO.Contact>? Contact { get; set; }
       


    }
}
