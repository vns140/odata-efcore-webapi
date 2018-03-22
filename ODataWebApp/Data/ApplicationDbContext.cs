using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ODataWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }

    public static class IServiceProviderExtensions
    {
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetService<ApplicationDbContext>())
            {
                db.Database.Migrate();

                if (db.Persons.Any())
                {
                    return;
                }

                db.Persons.AddRange(
                    new Person
                    {
                        Birthday = new DateTime(1950, 1, 12),
                        Name = "DOMINIK"
                    },
                    new Person
                    {
                        Birthday = new DateTime(1979, 6, 1),
                        Name = "KINGDOM"
                    },
                    new Person
                    {
                        Birthday = new DateTime(1984, 10, 2),
                        Name = "dominik"
                    },
                    new Person
                    {
                        Birthday = new DateTime(2000, 4, 22),
                        Name = "kingdom"
                    });

                db.SaveChanges();
            }
        }
    }
}