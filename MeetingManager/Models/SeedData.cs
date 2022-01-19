using MeetingManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace MeetingManager.Models
{
    public class SeedData
    {
        private static string SamplePassword = "123qwe"; 
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MeetingManagerContext(
                serviceProvider.GetRequiredService<DbContextOptions<MeetingManagerContext>>()))

            {
                if (context.User.Any())
                {
                    return;
                }

                context.User.AddRange(
                    new User { UserName = Faker.Internet.UserName(), EmailAddress = Faker.Internet.Email(), Password = BC.HashPassword(SamplePassword), UserDetail = new UserDetail()
                    {
                        Name = Faker.Name.First(),
                        SecondName = Faker.Name.Middle(),
                        LastName = Faker.Name.Last(),
                        Address = "",
                        Phone = Faker.Phone.Number(),
                        City = "",
                        Country = "",
                        Region = "",
                        PostCode = ""
                    }, Cart = new Cart()
                    }
                );

                context.SaveChanges();
            }
        }

    }
}
