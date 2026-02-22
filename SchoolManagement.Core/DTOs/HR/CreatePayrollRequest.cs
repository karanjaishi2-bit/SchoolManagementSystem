namespace SchoolManagement.Core.DTOs.HR
{
    public class CreatePayrollRequest
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string? PaymentDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}