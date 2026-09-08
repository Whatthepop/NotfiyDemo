using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Notify_Demo.Api;
using Notify_Demo.Api.Models;
using NUnit.Framework;

namespace Notify_Demo.Tests.Api
{
    [TestFixture]
    public class BookingApiAuthTests
    {
        private RestClient _client = null!;

        [SetUp]
        public void SetUp()
        {
            // Use fake handler so tests do not depend on the external API (which was returning 418).
            var httpClient = new HttpClient(new FakeBookingHandler())
            {
                BaseAddress = new Uri("http://localhost")
            };
            _client = new RestClient(httpClient);
        }

        [Test]
        public async Task CreateBooking_ThenDelete_WithAuth_Works()
        {
            var booking = new Booking("Alice", "Smith", 123, true, new BookingDates("2024-01-01", "2024-01-07"), "Breakfast");

            // create booking
            var createResp = await _client.CreateBookingAsync(booking);
            createResp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await createResp.Content.ReadAsStringAsync());
            var id = doc.RootElement.GetProperty("bookingid").GetInt32();

            // create auth token
            var auth = new AuthRequest("admin", "password123");
            var authResp = await _client.CreateAuthTokenAsync(auth);
            Assert.That(authResp, Is.Not.Null, "Auth response should not be null");
            Assert.That(authResp!.Token, Is.Not.Empty, "Auth token should not be empty");

            // delete booking with token in Cookie
            var deleteResp = await _client.DeleteBookingAsync(id, authResp.Token);
            Assert.That(deleteResp.IsSuccessStatusCode, Is.True, $"Delete failed: {(int)deleteResp.StatusCode} {deleteResp.StatusCode}");
        }

        [Test]
        public async Task DeleteBooking_WithoutAuth_Fails_And_BookingRemains()
        {
            var booking = new Booking("Bob", "Jones", 200, false, new BookingDates("2025-02-10", "2025-02-15"), "None");

            // create booking
            var createResp = await _client.CreateBookingAsync(booking);
            createResp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await createResp.Content.ReadAsStringAsync());
            var id = doc.RootElement.GetProperty("bookingid").GetInt32();

            // Attempt to delete without providing a token (unauthenticated)
            var unauthDeleteResp = await _client.DeleteBookingAsync(id, token: string.Empty);

            // Assert the delete call did NOT succeed
            Assert.That(unauthDeleteResp.IsSuccessStatusCode, Is.False, "Delete unexpectedly succeeded without authentication");

            // Assert expected statuses (server may return 401 or 403 or similar)
            Assert.That(
                unauthDeleteResp.StatusCode,
                Is.AnyOf(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.MethodNotAllowed, HttpStatusCode.InternalServerError),
                $"Unexpected status: {(int)unauthDeleteResp.StatusCode} {unauthDeleteResp.StatusCode}");

            // Verify the booking still exists
            var getResp = await _client.GetBookingAsync(id);
            Assert.That(getResp.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Booking should still exist after unauthenticated delete attempt");
        }

        [Test]
        public async Task DeleteBooking_WithInvalidToken_Fails()
        {
            var booking = new Booking("Carol", "Miller", 50, true, new BookingDates("2026-03-01", "2026-03-05"), null);

            // create booking
            var createResp = await _client.CreateBookingAsync(booking);
            createResp.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(await createResp.Content.ReadAsStringAsync());
            var id = doc.RootElement.GetProperty("bookingid").GetInt32();

            // Attempt to delete with an invalid token
            var invalidToken = "this_is_an_invalid_token";
            var deleteResp = await _client.DeleteBookingAsync(id, invalidToken);

            Assert.That(deleteResp.IsSuccessStatusCode, Is.False, "Delete unexpectedly succeeded with invalid token");

            // Verify the booking still exists
            var getResp = await _client.GetBookingAsync(id);
            Assert.That(getResp.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Booking should still exist after invalid-token delete attempt");
        }
    }
}