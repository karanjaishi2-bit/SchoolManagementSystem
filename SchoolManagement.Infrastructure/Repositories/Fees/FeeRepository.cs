using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Fees;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Fees
{
    public class FeeRepository : Repository<Fee>, IFeeRepository
    {
        public FeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Fee>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet.Where(f => f.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(f => f.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Fee>> GetOverdueFeesAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            return await _dbSet.Where(f =>
                f.Status != "Paid" &&
                string.Compare(f.DueDate, today) < 0
            ).ToListAsync();
        }

        public async Task<decimal> GetTotalPendingAmountAsync()
        {
            return await _dbSet
                .Where(f => f.Status == "Pending" || f.Status == "Overdue")
                .SumAsync(f => f.Amount);
        }
    }
}