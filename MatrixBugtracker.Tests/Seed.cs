using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.Tests
{
    internal static class Seed
    {
        public static List<User> Users { get; private set; }
        public static List<Product> Products { get; private set; }

        static Seed()
        {
            Users = new List<User> { 
                new User { 
                    Id = 1,
                    Email = "alice@example.com",
                    Role = UserRole.Admin,
                    FirstName = "Alice",
                    LastName = "A"
                },
                new User {
                    Id = 2,
                    Email = "bob@example.com",
                    Role = UserRole.Employee,
                    FirstName = "Bob",
                    LastName = "B"
                },
                new User {
                    Id = 3,
                    Email = "chris@example.com",
                    Role = UserRole.Employee,
                    FirstName = "Chris",
                    LastName = "C"
                },
                new User {
                    Id = 4,
                    Email = "dave@example.com",
                    Role = UserRole.Moderator,
                    FirstName = "Dave",
                    LastName = "D"
                },
                new User {
                    Id = 5,
                    Email = "elnara@example.com",
                    Role = UserRole.Moderator,
                    FirstName = "Elnara",
                    LastName = "E"
                },
                new User {
                    Id = 6,
                    Email = "flora@example.com",
                    Role = UserRole.Tester,
                    FirstName = "Flora",
                    LastName = "F"
                },
                new User {
                    Id = 7,
                    Email = "gunel@example.com",
                    Role = UserRole.Tester,
                    FirstName = "Gunel",
                    LastName = "G"
                }
            };

            Products = new List<Product> { 
                new Product
                {
                    Id = 1,
                    CreatorId = 2,
                    AccessLevel = ProductAccessLevel.Open,
                    Type = ProductType.PCApp,
                    Name = "PC app open"
                },
                new Product
                {
                    Id = 2,
                    CreatorId = 3,
                    AccessLevel = ProductAccessLevel.Closed,
                    Type = ProductType.MobileApp,
                    Name = "Mobile app closed"
                },
                new Product
                {
                    Id = 3,
                    CreatorId = 2,
                    AccessLevel = ProductAccessLevel.Secret,
                    Type = ProductType.WebSite,
                    Name = "Website secret"
                }
            };
        }
    }
}
