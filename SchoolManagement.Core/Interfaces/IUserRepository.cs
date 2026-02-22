using SchoolManagement.Core.Entities.Auth;

namespace SchoolManagement.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetByRoleAsync(string role);
    }
}