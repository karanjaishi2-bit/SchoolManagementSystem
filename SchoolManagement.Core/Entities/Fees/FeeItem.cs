namespace SchoolManagement.Core.Entities.Fees
{
    public class FeeItem : BaseEntity
    {
        public string FeeHead { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string FeeType { get; set; } = string.Empty; // Recurring, One-Time
        public string? Frequency { get; set; } // Monthly, Quarterly, Yearly
        public string? Description { get; set; }
        public int? FeeStructureId { get; set; } // Link to FeeStructure
        public int? FeeBillId { get; set; } // Link to FeeBill
    }
}