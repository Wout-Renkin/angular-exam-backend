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
              new Role { Name = "Journalist" },
              new Role { Name = "Admin" });
            context.SaveChanges();

            /*context.Users.AddRange(
                new User { UserID = 1, RoleID = 1, Username = "test", Password = "test", FirstName = "Test", LastName = "Test", Email = "test.test@thomasmore.be" }
                );*/

            context.Tags.AddRange(
                new Tag { Name = "Sport" },
                new Tag { Name = "Film" },
                new Tag { Name = "Reizen" },
                new Tag { Name = "Games" }
                );

            context.ArticleStatuses.AddRange(
                new ArticleStatus { Name = "Draft" },
                new ArticleStatus { Name = "To review" },
                new ArticleStatus { Name = "Published" }
                );

            /*context.Articles.AddRange(
                new Article { UserID = 1, Title = "Messi verlaat FC Barçelona", SubTitle = "Messi stuurde een fax met de boodschap dat hij wilt vertrekken.", ArticleStatusID = 1, TagID = 1, Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus consequat non justo dignissim varius. Morbi finibus magna non neque bibendum efficitur. Aliquam eu auctor sem, ut mollis erat. Donec ornare dolor ex, tincidunt blandit purus sodales id. Phasellus a hendrerit libero. Nunc eu ultrices libero. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer consequat egestas dui sit amet dignissim. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. In sit amet cursus elit, eu dignissim elit. Ut aliquam cursus urna ultricies rhoncus. Proin vitae neque erat. Sed mollis consectetur diam eget vestibulum." }
                );*/

            context.SaveChanges();
        }
    }
}
