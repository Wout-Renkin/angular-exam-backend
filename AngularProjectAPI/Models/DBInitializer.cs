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
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            context.Roles.AddRange(
              new Role { Name = "User" },
              new Role { Name = "Employee" },
              new Role { Name = "Moderator" },
              new Role { Name = "SuperAdmin" });
            context.Users.AddRange(
                new User { FirstName = "Jeff", LastName = "Janssens", Email = "Jeff@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Jelle", LastName = "Jellens", Email = "Jelle@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Simon", LastName = "Simonsen", Email = "Simon@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Jeffrey", LastName = "Jeffi", Email = "Jeffrey@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Julia", LastName = "Moch", Email = "Julia@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Elke", LastName = "Elkes", Email = "Elke@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Gregory", LastName = "Gregoirs", Email = "Gregory@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Senna", LastName = "Sennas", Email = "Senna@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Kristof", LastName = "Kristoffus", Email = "Kristof@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Senne", LastName = "Sennekes", Email = "Senne@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Kenny", LastName = "Kenji", Email = "Kenny@hotmail.com", Password = "secret", RoleId = 1 },
                new User { FirstName = "Morgan", LastName = "Freeman", Email = "Morgan@hotmail.com", Password = "secret", RoleId = 1 }
                );
            context.SaveChanges();

            /* context.Roles.AddRange(
               new Role { Name = "User" },
               new Role { Name = "Journalist" },
               new Role { Name = "Admin" });
             context.SaveChanges();

             context.Users.AddRange(
                 new User { UserID = 1, RoleID = 1, Username = "test", Password = "test", FirstName = "Test", LastName = "Test", Email = "test.test@thomasmore.be" }
                 );

             context.Companies.AddRange(
                 new Company { City="Wiekevorst", Name="test", Street="Heeredreef", StreetNumber = 4, UserID = 1, CompanyId= 1}
                 );

             context.Groups.AddRange(
                 new Group { Name="HR", CompanyId= 1, GroupId = 1 });

             context.Posts.AddRange(
                 new Post { Body = "hey", CompanyId = 1, GroupId = 1, UserId = 1});*/

            /*context.Articles.AddRange(
                new Article { UserID = 1, Title = "Messi verlaat FC Barçelona", SubTitle = "Messi stuurde een fax met de boodschap dat hij wilt vertrekken.", ArticleStatusID = 1, TagID = 1, Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consequat non justo dignissim varius. Morbi finibus magna non neque bibendum efficitur. Aliquam eu auctor sem, ut mollis erat. Donec ornare dolor ex, tincidunt blandit purus sodales id. Phasellus a hendrerit libero. Nunc eu ultrices libero. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer consequat egestas dui sit amet dignissim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In sit amet cursus elit, eu dignissim elit. Ut aliquam cursus urna ultricies rhoncus. Proin vitae neque erat. Sed mollis consectetur diam eget vestibulum." }
                );*/

            context.SaveChanges();
        }
    }
}
