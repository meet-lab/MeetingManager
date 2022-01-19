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
                   new User
                   {
                       UserName = "example",
                       EmailAddress = "example@testdomain.com",
                       Password = BC.HashPassword(SamplePassword),
                       UserDetail = new UserDetail()
                       {
                           Name = Faker.Name.First(),
                           SecondName = Faker.Name.Middle(),
                           LastName = Faker.Name.Last(),
                           Address = Faker.Address.StreetAddress(),
                           Phone = Faker.Phone.Number(),
                           City = Faker.Address.City(),
                           Country = Faker.Address.Country(),
                           Region = Faker.Address.UsTerritory(),
                           PostCode = Faker.Address.ZipCode()
                       },
                       Cart = new Cart()
                   },
                   new User
                   {
                       UserName = "user",
                       EmailAddress = "user@testdomain.com",
                       Password = BC.HashPassword(SamplePassword),
                       UserDetail = new UserDetail()
                       {
                           Name = Faker.Name.First(),
                           SecondName = Faker.Name.Middle(),
                           LastName = Faker.Name.Last(),
                           Address = Faker.Address.StreetAddress(),
                           Phone = Faker.Phone.Number(),
                           City = Faker.Address.City(),
                           Country = Faker.Address.Country(),
                           Region = Faker.Address.UsTerritory(),
                           PostCode = Faker.Address.ZipCode()
                       },
                       Cart = new Cart()
                   },
                   new User
                   {
                       UserName = Faker.Name.First(),
                       EmailAddress = Faker.Internet.Email(),
                       Password = BC.HashPassword(SamplePassword),
                       UserDetail = new UserDetail()
                       {
                           Name = Faker.Name.First(),
                           SecondName = Faker.Name.Middle(),
                           LastName = Faker.Name.Last(),
                           Address = Faker.Address.StreetAddress(),
                           Phone = Faker.Phone.Number(),
                           City = Faker.Address.City(),
                           Country = Faker.Address.Country(),
                           Region = Faker.Address.UsTerritory(),
                           PostCode = Faker.Address.ZipCode()
                       },
                       Cart = new Cart()
                   }
               );

                context.SaveChanges();

                context.Offer.AddRange(
                        new Offer
                        {
                            UserId = 1,
                            Title = "Sala w siedzibie OSP",
                            Description = "Spędź wieczór ze swoimi znajomymi w wyjątkowym miejscu, urząź niezwykłą imprezę już dziś!",
                            Price = 250,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                         {
                            UserId = 1,
                            Title = "Sala balowa!",
                            Description = "Urządź niezapomniane przyjęcie w jedynej takiej sali w Polsce! Twoi znajomi będą się tutaj dobrze bawić!",
                            Price = 660,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 1,
                            Title = "Piknik na polanie",
                            Description = "Idealne miejsce na romatyczną randkę...",
                            Price = 95,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 1,
                            Title = "Pokój Hotelowy",
                            Description = "Doba hotelowa w naszym hotelu to coś czego potrzebujesz żeby wypocząć!",
                            Price = 320,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 2,
                            Title = "Tunel aerodynamiczny",
                            Description = "W tym miejscu możesz ciekawie spędzić wolną chwilę. Idealne dla osób które nie boją się wyzwań!",
                            Price = 600,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 2,
                            Title = "Tor gokartowy",
                            Description = "Wynajmij tor gokartowy, do dyspozycji wykfalifikowany personel.",
                            Price = 900,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 3,
                            Title = "Miejsce w garażu",
                            Description = "Wynajmij miejsce w garaży w okolicach rynku. Idelne na krótkie pobyty. TANIO!",
                            Price = 113,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        },
                        new Offer
                        {
                            UserId = 3,
                            Title = "Przejazd konno po dolinie",
                            Description = "Niezapomniana wycieczka konna po dolinach w okolicach Krakowa. Super przygoda dla grupy znajomych!",
                            Price = 305,
                            Status = "published",
                            CreatedAt = DateTime.Now,
                        }
                    );

                context.SaveChanges();

                context.Order.AddRange(
                    new Order
                    {
                        UserId = 1,
                        OfferId = 5,
                        Comment = "Proszę przygotować kostium w rozmiarze XL.",
                        CreateDate = DateTime.Now,
                        Status = "Created",
                        From = DateTime.Now,
                        To = DateTime.Now.AddDays(1),
                        Amount = 600
                    },
                    new Order
                    {
                        UserId = 2,
                        OfferId = 3,
                        Comment = "Alergia na truskawki.",
                        CreateDate = DateTime.Now,
                        Status = "Created",
                        From = DateTime.Now.AddDays(3),
                        To = DateTime.Now.AddDays(4),
                        Amount = 95
                    },
                    new Order
                    {
                        UserId = 2,
                        OfferId = 1,
                        Comment = "",
                        CreateDate = DateTime.Now,
                        Status = "Canceled",
                        From = DateTime.Now.AddDays(2),
                        To = DateTime.Now.AddDays(4),
                        Amount = 500
                    },
                    new Order
                    {
                        UserId = 1,
                        OfferId = 6,
                        Comment = "Przyjęcie urodzinowe dla znajomego",
                        CreateDate = DateTime.Now,
                        Status = "Canceled",
                        From = DateTime.Now.AddDays(-5),
                        To = DateTime.Now.AddDays(-4),
                        Amount = 900
                    }
                );

                context.SaveChanges();

                context.CartLineItem.AddRange(
                    new CartLineItem
                    {
                        CartId = 1,
                        From = DateTime.Now.AddDays(-1),
                        To = DateTime.Now.AddDays(1),
                        OfferId = 8,
                        Name = "Przejazd konno po dolinie",
                        TotalPrice = 610
                    },
                    new CartLineItem
                    {
                        CartId = 2,
                        From = DateTime.Now,
                        To = DateTime.Now.AddDays(2),
                        OfferId = 7,
                        Name = "Miejsce w garażu",
                        TotalPrice = 226
                    },
                    new CartLineItem
                    {
                        CartId = 2,
                        From = DateTime.Now.AddDays(7),
                        To = DateTime.Now.AddDays(10),
                        OfferId = 4,
                        Name = "Pokój Hotelowy",
                        TotalPrice = 960
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
