namespace SchoolManagement.Core.Entities.HR
{
    public class Payroll : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeType { get; set; } = string.Empty; // Teacher, Staff
        public string Month { get; set; } = string.Empty; // YYYY-MM
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string? PaymentDate { get; set; }
        public string Status { get; set; } = "Pending"; // Paid, Pending, On Hold
    }
}