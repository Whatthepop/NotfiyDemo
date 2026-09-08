using System.Text.Json.Serialization;

namespace Notify_Demo.Api.Models
{
    public record BookingDates([property: JsonPropertyName("checkin")] string CheckIn, [property: JsonPropertyName("checkout")] string CheckOut);

    public record Booking(
        [property: JsonPropertyName("firstname")] string FirstName,
        [property: JsonPropertyName("lastname")] string LastName,
        [property: JsonPropertyName("totalprice")] int TotalPrice,
        [property: JsonPropertyName("depositpaid")] bool DepositPaid,
        [property: JsonPropertyName("bookingdates")] BookingDates BookingDates,
        [property: JsonPropertyName("additionalneeds")] string? AdditionalNeeds);
}