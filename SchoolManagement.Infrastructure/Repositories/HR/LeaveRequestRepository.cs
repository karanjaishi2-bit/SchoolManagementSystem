using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.HR;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.HR
{
    public class LeaveRequestRepository : Repository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.Where(l => l.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeTypeAsync(string employeeType)
        {
            return await _dbSet.Where(l => l.EmployeeType == employeeType).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(l => l.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync()
        {
            return await _dbSet.Where(l => l.Status == "Pending").ToListAsync();
        }
    }
}