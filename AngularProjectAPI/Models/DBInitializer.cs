using AngularProjectAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class DBInitializer
    {
        public static void Initialize(NewsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any user.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            context.Roles.AddRange(
              new Role { RoleID = 1, Name = "User" },
              new Role { RoleID = 2, Name = "Journalist" },
              new Role { RoleID = 3, Name = "Admin" });
            context.SaveChanges();

            context.Users.AddRange(
                new User { RoleID = 1, Username = "test", Password = "test", FirstName = "Test", LastName = "Test", Email = "test.test@thomasmore.be" }
                );

            context.SaveChanges();
        }
    }
}
