using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Results;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Results
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet.Where(g => g.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetBySubjectAsync(string subject)
        {
            return await _dbSet.Where(g => g.Subject == subject).ToListAsync();
        }
    }
}