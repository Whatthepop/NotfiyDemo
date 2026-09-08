using Microsoft.EntityFrameworkCore;

namespace Notify_Demo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>(b =>
            {
                b.ToTable("Bookings");
                b.HasKey(x => x.Id);
                b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(100);
                b.Property(x => x.CheckIn).IsRequired();
                b.Property(x => x.CheckOut).IsRequired();
            });
        }
    }
}