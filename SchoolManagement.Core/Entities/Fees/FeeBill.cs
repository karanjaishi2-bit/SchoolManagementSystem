namespace SchoolManagement.Core.Entities.Fees
{
    public class FeeBill : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string BillDate { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Paid, Partial, Pending, Overdue

        // Navigation Properties
        public ICollection<FeeItem>? FeeItems { get; set; }
    }
}