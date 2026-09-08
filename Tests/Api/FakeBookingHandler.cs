using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Notify_Demo.Tests.Api
{
    // Minimal in-memory fake HTTP handler for the external booking API.
    // Keeps tests deterministic and avoids reliance on the external service that returned 418.
    public class FakeBookingHandler : HttpMessageHandler
    {
        private static int _idCounter = 0;
        private static readonly ConcurrentDictionary<int, string> _store = new();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath ?? "/";
            HttpResponseMessage resp;

            if (request.Method == HttpMethod.Post && path.Equals("/booking", StringComparison.OrdinalIgnoreCase))
            {
                var content = request.Content?.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult() ?? "{}";
                var id = Interlocked.Increment(ref _idCounter);
                _store[id] = content;

                var body = JsonSerializer.Serialize(new
                {
                    bookingid = id,
                    booking = JsonSerializer.Deserialize<JsonElement>(content)
                });

                resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                return Task.FromResult(resp);
            }

            if (request.Method == HttpMethod.Get && path.StartsWith("/booking/", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(path.Substring("/booking/".Length), out var id) && _store.TryGetValue(id, out var stored))
                {
                    resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(stored, Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                return Task.FromResult(resp);
            }

            if (request.Method == HttpMethod.Post && path.Equals("/auth", StringComparison.OrdinalIgnoreCase))
            {
                // Always return a valid token for tests
                var body = JsonSerializer.Serialize(new { token = "valid-token" });
                resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                return Task.FromResult(resp);
            }

            if (request.Method == HttpMethod.Delete && path.StartsWith("/booking/", StringComparison.OrdinalIgnoreCase))
            {
                // Check Cookie header for token
                var hasCookie = request.Headers.TryGetValues("Cookie", out var cookieValues);
                var token = string.Empty;
                if (hasCookie)
                {
                    foreach (var c in cookieValues)
                    {
                        // expecting format token={value}
                        var parts = c.Split(';');
                        foreach (var p in parts)
                        {
                            var kv = p.Split('=', 2);
                            if (kv.Length == 2 && kv[0].Trim().Equals("token", StringComparison.OrdinalIgnoreCase))
                            {
                                token = kv[1].Trim();
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(token)) break;
                    }
                }

                if (string.IsNullOrEmpty(token) || token != "valid-token")
                {
                    // Unauthenticated / invalid token
                    resp = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    return Task.FromResult(resp);
                }

                if (int.TryParse(path.Substring("/booking/".Length), out var id))
                {
                    var removed = _store.TryRemove(id, out _);
                    resp = new HttpResponseMessage(removed ? HttpStatusCode.Created : HttpStatusCode.NotFound);
                }
                else
                {
                    resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                return Task.FromResult(resp);
            }

            // Fallback: return 404
            resp = new HttpResponseMessage(HttpStatusCode.NotFound);
            return Task.FromResult(resp);
        }
    }
}