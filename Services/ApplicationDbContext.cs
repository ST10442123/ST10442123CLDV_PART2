using EventEase3.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEase3.Services
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }

    }
}
