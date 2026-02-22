using SchoolManagement.Core.Entities.Results;

namespace SchoolManagement.Core.Interfaces
{
    public interface IGradeRepository : IRepository<Grade>
    {
        Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Grade>> GetBySubjectAsync(string subject);
    }
}