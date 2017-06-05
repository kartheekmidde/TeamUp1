namespace TeamUp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TeamUp1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TeamUp1.Models.ApplicationDbContext context)
        {

            // ADD ROLE NAMES IN THIS SECTION. YOU DO NOT HAVE TO USE THE 3 PROVIDED
            // HERE AND CAN USE ROLE NAMES THAT ARE APPROPRIATE FOR YOUR APPLICATION
            var roles = new[]
            {
                "Admin",
                "Player",
                "Editor"
            };

            // USE THE FOLLOWING PATTERN TO ADD DEFAULT USERS TO YOUR SYSTEM
            // ROLES CAN BE COMMA SEPERATED TO ADD MULTIPLE ROLES
            // ROLES PROVIDED MUST EXIST IN THE LIST ABOVE
            var users = new[]
            {
                new {Email = "steve@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "kartheek@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "dheeraj@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "anusha@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "reader@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "admin@example.com", Pwd = "Password123", Roles = "Player"},
                new {Email = "editor@example.com", Pwd = "Password123", Roles = "Player"},
            };

            // DO NOT MODIFY THE CODE BELOW THIS LINE
            roles.ToList().ForEach(r => context.Roles.AddOrUpdate(x => x.Name,
            new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = r }));
            foreach (var user in users)
            {
                ApplicationUserManager mgr = new ApplicationUserManager(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<Models.ApplicationUser>(context));
                Models.ApplicationUser existingUser = context.Users.FirstOrDefault(x => x.UserName == user.Email);
                if (existingUser != null) Microsoft.AspNet.Identity.UserManagerExtensions.Delete(mgr, existingUser);
                Models.ApplicationUser au = new Models.ApplicationUser { Email = user.Email, UserName = user.Email };
                var result = mgr.CreateAsync(au, user.Pwd).Result;
                if (!string.IsNullOrEmpty(user.Roles))
                    Microsoft.AspNet.Identity.UserManagerExtensions.AddToRoles(mgr, au.Id, user.Roles.Split(','));
            }
        }
    }
}
