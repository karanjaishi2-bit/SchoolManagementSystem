using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Staff;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Staff
{
    public class StaffRepository : Repository<Core.Entities.Staff.Staff>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Core.Entities.Staff.Staff?> GetByStaffIdAsync(string staffId)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.StaffId == staffId);
        }

        public async Task<Core.Entities.Staff.Staff?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Core.Entities.Staff.Staff>> GetByDepartmentAsync(string department)
        {
            return await _dbSet.Where(s => s.Department == department).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Staff.Staff>> GetByRoleAsync(string role)
        {
            return await _dbSet.Where(s => s.Role == role).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Staff.Staff>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(s => s.Status == status).ToListAsync();
        }
    }
}