namespace SchoolManagement.Core.Entities.Fees
{
    public class Fee : BaseEntity
    {
        public string StudentName { get; set; } = string.Empty;
        public int? StudentId { get; set; }
        public decimal Amount { get; set; }
        public string DueDate { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Paid, Pending, Overdue
        public string? ClassId { get; set; }
    }
}