namespace SchoolManagement.Core.DTOs.Fees
{
    public class FeeItemRequest
    {
        public int? Id { get; set; }
        public string FeeHead { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string FeeType { get; set; } = string.Empty; // Recurring, One-Time
        public string? Frequency { get; set; } // Monthly, Quarterly, Yearly
        public string? Description { get; set; }
    }
}