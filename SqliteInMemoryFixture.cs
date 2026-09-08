using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Notify_Demo.Data;

namespace Notify_Demo.Tests.Database
{
    public sealed class SqliteInMemoryFixture : IDisposable
    {
        public AppDbContext Context { get; }
        private readonly SqliteConnection _connection;

        public SqliteInMemoryFixture()
        {
            // In-memory SQLite needs an open connection to survive context disposal
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new AppDbContext(options);
            Context.Database.EnsureCreated(); // or Context.Database.Migrate() if you want migrations applied
        }

        public void Dispose()
        {
            Context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}