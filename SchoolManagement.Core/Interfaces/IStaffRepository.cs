using SchoolManagement.Core.Entities.Staff;

namespace SchoolManagement.Core.Interfaces
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByStaffIdAsync(string staffId);
        Task<Staff?> GetByEmailAsync(string email);
        Task<IEnumerable<Staff>> GetByDepartmentAsync(string department);
        Task<IEnumerable<Staff>> GetByRoleAsync(string role);
        Task<IEnumerable<Staff>> GetByStatusAsync(string status);
    }
}