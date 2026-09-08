using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Notify_Demo.Api.Models;

namespace Notify_Demo.Api
{
    public class RestClient
    {
        private readonly HttpClient _http;

        public RestClient(string baseUrl = "https://restful-booker.herokuapp.com")
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        // New ctor to allow injecting a pre-configured HttpClient (used by tests)
        public RestClient(HttpClient httpClient)
        {
            _http = httpClient ?? new HttpClient { BaseAddress = new Uri("https://restful-booker.herokuapp.com") };
        }

        public async Task<HttpResponseMessage> CreateBookingAsync(Booking booking) =>
            await _http.PostAsJsonAsync("/booking", booking);

        public async Task<HttpResponseMessage> GetBookingAsync(int id) =>
            await _http.GetAsync($"/booking/{id}");

        // New: create an auth token (POST /auth)
        public async Task<AuthResponse?> CreateAuthTokenAsync(AuthRequest auth)
        {
            var resp = await _http.PostAsJsonAsync("/auth", auth);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AuthResponse>();
        }

        // New: delete booking using token passed in Cookie header
        public async Task<HttpResponseMessage> DeleteBookingAsync(int id, string token)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"/booking/{id}");
            // Restful-Booker expects token in Cookie header: token={value}
            req.Headers.Add("Cookie", $"token={token}");
            return await _http.SendAsync(req);
        }
    }
}