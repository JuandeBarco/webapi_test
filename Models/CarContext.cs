using Microsoft.EntityFrameworkCore;

namespace webapi_test.Models
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options)
            : base(options)
        {
        }

        public DbSet<CarItem> CarItems { get; set; } = null!;
    }
}