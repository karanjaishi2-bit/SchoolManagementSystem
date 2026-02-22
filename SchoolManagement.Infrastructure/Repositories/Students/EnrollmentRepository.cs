using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Students;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Students
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet.Where(e => e.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetByCourseAsync(string course)
        {
            return await _dbSet.Where(e => e.Course == course).ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(e => e.Status == status).ToListAsync();
        }
    }
}