namespace SchoolManagement.Core.DTOs.Fees
{
    public class CreateFeeBillRequest
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string BillDate { get; set; } = string.Empty;
        public string DueDate { get; set; } = string.Empty;
        public List<FeeItemRequest> FeeItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; } = "Pending";
    }
}