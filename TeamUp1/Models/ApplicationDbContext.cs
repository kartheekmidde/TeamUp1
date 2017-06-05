using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace TeamUp1.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Event> Events { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<TeamUp1.Models.SignUp> SignUps { get; set; }
    }
}