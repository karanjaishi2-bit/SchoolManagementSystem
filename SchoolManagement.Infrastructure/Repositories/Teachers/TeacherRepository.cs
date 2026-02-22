using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Teachers;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Teachers
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Teacher?> GetByTeacherIdAsync(string teacherId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);
        }

        public async Task<Teacher?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Email == email);
        }

        public async Task<IEnumerable<Teacher>> GetBySubjectAsync(string subject)
        {
            return await _dbSet
                .Where(t => t.Subject == subject)
                .ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(t => t.Status == status)
                .ToListAsync();
        }
    }
}