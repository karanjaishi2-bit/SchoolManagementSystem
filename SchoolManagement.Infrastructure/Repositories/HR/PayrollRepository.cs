using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.HR;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.HR
{
    public class PayrollRepository : Repository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payroll>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.Where(p => p.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<IEnumerable<Payroll>> GetByEmployeeTypeAsync(string employeeType)
        {
            return await _dbSet.Where(p => p.EmployeeType == employeeType).ToListAsync();
        }

        public async Task<IEnumerable<Payroll>> GetByMonthAsync(string month)
        {
            return await _dbSet.Where(p => p.Month == month).ToListAsync();
        }

        public async Task<IEnumerable<Payroll>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(p => p.Status == status).ToListAsync();
        }

        public async Task<decimal> GetTotalPayrollByMonthAsync(string month)
        {
            return await _dbSet.Where(p => p.Month == month).SumAsync(p => p.NetSalary);
        }
    }
}