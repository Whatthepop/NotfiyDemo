
using NUnit.Framework;
using Notify_Demo.Data;

namespace Notify_Demo.Tests.Database
{
    [TestFixture]
    public class BookingRepositoryTests
    {
        private SqliteInMemoryFixture _fixture = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new SqliteInMemoryFixture();
        }

        [TearDown]
        public void TearDown()
        {
            _fixture.Dispose();
        }

        [Test]
        public void CanAddAndReadBooking()
        {
            var ctx = _fixture.Context;
            var ent = new BookingEntity
            {
                FirstName = "T",
                LastName = "User",
                TotalPrice = 10,
                DepositPaid = false,
                CheckIn = "2026-01-01",
                CheckOut = "2026-01-02"
            };

            ctx.Bookings.Add(ent);
            ctx.SaveChanges();

            var fromDb = ctx.Bookings.Find(ent.Id);
            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.FirstName, Is.EqualTo("T"));
        }
    }
}