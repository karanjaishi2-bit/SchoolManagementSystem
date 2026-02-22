using SchoolManagement.Core.Entities.Teachers;

namespace SchoolManagement.Core.Interfaces
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher?> GetByTeacherIdAsync(string teacherId);
        Task<Teacher?> GetByEmailAsync(string email);
        Task<IEnumerable<Teacher>> GetBySubjectAsync(string subject);
        Task<IEnumerable<Teacher>> GetByStatusAsync(string status);
    }
}