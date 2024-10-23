using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class FlightTicketContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=FlightTicket;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;")
                .LogTo(Console.WriteLine, LogLevel.Information); // Konsola yazdır
            ;
        }
        public DbSet<Airport> Airports { get; set; }
    }
}
