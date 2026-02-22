using SchoolManagement.Core.Entities.Subjects;

namespace SchoolManagement.Core.Interfaces
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Subject?> GetByCodeAsync(string code);
        Task<IEnumerable<Subject>> GetByStatusAsync(string status);
        Task<IEnumerable<Subject>> GetByTeacherIdAsync(int teacherId);
        Task<IEnumerable<Subject>> GetByClassIdAsync(string classId);
    }
}