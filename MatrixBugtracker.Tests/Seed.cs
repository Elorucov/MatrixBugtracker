using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.Tests
{
    internal static class Seed
    {
        public static List<User> Users { get; private set; }
        public static List<Product> Products { get; private set; }
        public static List<Report> Reports { get; private set; }

        static Seed()
        {
            Users = GetUsers();
            Products = GetProducts();
            Reports = GetReports();
        }

        private static List<User> GetUsers() => new List<User> {
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

        private static List<Product> GetProducts() => new List<Product> {
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

        private static List<Report> GetReports() => new List<Report>
        {
            new Report
            {
                Id = 1,
                ProductId = 3,
                CreatorId = 6,
                Title = "Sample report for secret product created by user 6",
                Steps = "Steps",
                Actual = "Actual",
                Supposed = "Supposed",
                Severity = ReportSeverity.Medium,
                ProblemType = ReportProblemType.FunctionNotWorking,
                Status = ReportStatus.Open,
                Reproduces = new List<ReportReproduce>()
            },
            new Report
            {
                Id = 2,
                ProductId = 2,
                CreatorId = 6,
                Title = "Sample report for closed product created by user 6",
                Steps = "Steps",
                Actual = "Actual",
                Supposed = "Supposed",
                Severity = ReportSeverity.Medium,
                ProblemType = ReportProblemType.FunctionNotWorking,
                Status = ReportStatus.Open,
                Reproduces = new List<ReportReproduce>
                {
                    new ReportReproduce
                    {
                        UserId = 7, ReportId = 1
                    }
                }
            },
            new Report
            {
                Id = 3,
                ProductId = 1,
                CreatorId = 6,
                Title = "Sample report for open product created by user 6",
                Steps = "Steps",
                Actual = "Actual",
                Supposed = "Supposed",
                Severity = ReportSeverity.Medium,
                ProblemType = ReportProblemType.FunctionNotWorking,
                Status = ReportStatus.Open,
                Reproduces = new List<ReportReproduce>
                {
                    new ReportReproduce
                    {
                        UserId = 7, ReportId = 1
                    }
                }
            }
        };
    }
}
