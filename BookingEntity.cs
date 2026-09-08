namespace Notify_Demo.Data
{
    public class BookingEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int TotalPrice { get; set; }
        public bool DepositPaid { get; set; }
        public string CheckIn { get; set; } = null!;
        public string CheckOut { get; set; } = null!;
        public string? AdditionalNeeds { get; set; }
    }
}