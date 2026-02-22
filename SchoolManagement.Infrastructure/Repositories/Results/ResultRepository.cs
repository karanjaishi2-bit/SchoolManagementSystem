using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Results;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Results
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId)
        {
            return await _dbSet.Where(r => r.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<Result>> GetByClassAsync(string className)
        {
            return await _dbSet.Where(r => r.ClassName == className).ToListAsync();
        }
    }
}