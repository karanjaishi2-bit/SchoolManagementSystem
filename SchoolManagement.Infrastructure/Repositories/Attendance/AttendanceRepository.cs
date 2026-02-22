using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Attendance;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Attendance
{
    public class AttendanceRepository : Repository<Core.Entities.Attendance.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByEntityIdAsync(int entityId)
        {
            return await _dbSet.Where(a => a.EntityId == entityId).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByEntityTypeAsync(string entityType)
        {
            return await _dbSet.Where(a => a.EntityType == entityType).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByDateAsync(string date)
        {
            return await _dbSet.Where(a => a.Date == date).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByDateRangeAsync(string startDate, string endDate)
        {
            return await _dbSet.Where(a =>
                string.Compare(a.Date, startDate) >= 0 &&
                string.Compare(a.Date, endDate) <= 0
            ).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(a => a.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Core.Entities.Attendance.Attendance>> GetByClassIdAsync(string classId)
        {
            return await _dbSet.Where(a => a.ClassId == classId).ToListAsync();
        }
    }
}