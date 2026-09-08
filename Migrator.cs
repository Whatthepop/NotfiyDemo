using Microsoft.EntityFrameworkCore;

namespace Notify_Demo.Data
{
    public static class Migrator
    {
        public static void Migrate(DbContextOptions<AppDbContext> options)
        {
            using var ctx = new AppDbContext(options);
            ctx.Database.Migrate();
        }
    }
}