using System.Text.Json.Serialization;

namespace Notify_Demo.Api.Models
{
    public record AuthResponse(
        [property: JsonPropertyName("token")] string Token
    );
}