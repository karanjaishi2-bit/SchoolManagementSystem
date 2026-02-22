using SchoolManagement.Core.Entities.Students;

namespace SchoolManagement.Core.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByStudentIdAsync(string studentId);
        Task<Student?> GetByEmailAsync(string email);
        Task<IEnumerable<Student>> GetByClassAsync(string className);
        Task<IEnumerable<Student>> GetByStatusAsync(string status);
        Task<IEnumerable<Student>> SearchAsync(string searchTerm);
    }
}