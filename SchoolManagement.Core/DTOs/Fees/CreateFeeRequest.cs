namespace SchoolManagement.Core.DTOs.Fees
{
    public class CreateFeeRequest
    {
        public string StudentName { get; set; } = string.Empty;
        public int? StudentId { get; set; }
        public decimal Amount { get; set; }
        public string DueDate { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? ClassId { get; set; }
    }
}