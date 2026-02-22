using SchoolManagement.Core.Entities.HR;

namespace SchoolManagement.Core.Interfaces
{
    public interface IPayrollRepository : IRepository<Payroll>
    {
        Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Payroll>> GetByEmployeeTypeAsync(string employeeType);
        Task<IEnumerable<Payroll>> GetByMonthAsync(string month);
        Task<IEnumerable<Payroll>> GetByStatusAsync(string status);
        Task<decimal> GetTotalPayrollByMonthAsync(string month);
    }
}