using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Subjects;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Subjects
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Subject?> GetByCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<IEnumerable<Subject>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(s => s.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetByTeacherIdAsync(int teacherId)
        {
            return await _dbSet.Where(s => s.TeacherId == teacherId).ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetByClassIdAsync(string classId)
        {
            return await _dbSet.Where(s => s.ClassId == classId).ToListAsync();
        }
    }
}