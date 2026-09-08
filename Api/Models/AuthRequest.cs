using System.Text.Json.Serialization;

namespace Notify_Demo.Api.Models
{
    public record AuthRequest(
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("password")] string Password
    );
}