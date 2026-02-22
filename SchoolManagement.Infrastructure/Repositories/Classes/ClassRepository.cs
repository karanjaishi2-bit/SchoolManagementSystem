using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Classes;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Classes
{
    public class ClassRepository : Repository<ClassData>, IClassRepository
    {
        public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ClassData?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<ClassData>> GetByStatusAsync(string status)
        {
            return await _dbSet.Where(c => c.Status == status).ToListAsync();
        }
    }
}