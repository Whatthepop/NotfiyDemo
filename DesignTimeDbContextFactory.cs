using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notify_Demo.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            // Use a simple local file-based SQLite DB for design-time migration commands
            var conn = "Data Source=notifydemo.db";
            builder.UseSqlite(conn);
            return new AppDbContext(builder.Options);
        }
    }
}