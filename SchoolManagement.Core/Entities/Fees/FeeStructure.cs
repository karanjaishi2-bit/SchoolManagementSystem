namespace SchoolManagement.Core.Entities.Fees
{
    public class FeeStructure : BaseEntity
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Active"; // Active, Inactive

        // Navigation Properties
        public ICollection<FeeItem>? FeeItems { get; set; }
    }
}