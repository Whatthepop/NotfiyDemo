using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Notify_Demo.Api;
using Notify_Demo.Api.Models;
using NUnit.Framework;

namespace Notify_Demo.Tests.Api
{ 
    [TestFixture]
    public class BookingApiTests
    {
        private RestClient _client = null!;

        [SetUp]
        public void SetUp() => _client = new RestClient();

        [Test]
        public async Task CreateBooking_ThenGet_VerifiesContent()
        {
            var booking = new Booking("Alice", "Smith", 123, true, new BookingDates("2024-01-01", "2024-01-07"), "Breakfast");

            var createResp = await _client.CreateBookingAsync(booking);
            createResp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await createResp.Content.ReadAsStringAsync());
            var id = doc.RootElement.GetProperty("bookingid").GetInt32();

            var getResp = await _client.GetBookingAsync(id);
            Assert.That(getResp.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Expected GET to return 200 OK");

            var returned = await getResp.Content.ReadFromJsonAsync<Booking>();
            Assert.That(returned, Is.Not.Null, "Returned booking should not be null");
            Assert.That(returned!.FirstName, Is.EqualTo(booking.FirstName), $"FirstName mismatch. Expected: {booking.FirstName}, Actual: {returned.FirstName}");
            Assert.That(returned.LastName, Is.EqualTo(booking.LastName), $"LastName mismatch. Expected: {booking.LastName}, Actual: {returned.LastName}");
        }

        [Test]
        public async Task GetNonExistentBooking_Returns404()
        {
            var resp = await _client.GetBookingAsync(999999); // likely missing
            Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.NotFound), "Expected 404 for non-existent booking");
        }

    }
}