using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Auth;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.Infrastructure.Repositories.Auth
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(string role)
        {
            return await _dbSet.Where(u => u.Role == role).ToListAsync();
        }
    }
}