namespace SchoolManagement.Core.DTOs.HR
{
    public class CreateLeaveRequestRequest
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty;
        public string LeaveType { get; set; } = string.Empty;
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
    }
}