using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Students;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Students
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Student?> GetByStudentIdAsync(string studentId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Student>> GetByClassAsync(string className)
        {
            return await _dbSet
                .Where(s => s.Class == className)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> SearchAsync(string searchTerm)
        {
            return await _dbSet
                .Where(s => s.Name.Contains(searchTerm) ||
                           s.StudentId.Contains(searchTerm) ||
                           s.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}