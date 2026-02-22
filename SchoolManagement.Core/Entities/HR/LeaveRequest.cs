namespace SchoolManagement.Core.Entities.HR
{
    public class LeaveRequest : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty; // Teacher, Staff
        public string LeaveType { get; set; } = string.Empty; // Sick, Casual, Annual, Unpaid, Maternity, Paternity
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
        public string? ApprovedBy { get; set; }
    }
}