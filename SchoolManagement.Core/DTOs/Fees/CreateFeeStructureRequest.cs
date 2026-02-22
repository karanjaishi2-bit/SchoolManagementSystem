namespace SchoolManagement.Core.DTOs.Fees
{
    public class CreateFeeStructureRequest
    {
        public string ClassId { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public List<FeeItemRequest> RecurringItems { get; set; } = new();
        public List<FeeItemRequest> OneTimeItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Active";
    }
}