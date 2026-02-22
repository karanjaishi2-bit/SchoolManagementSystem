using SchoolManagement.Core.Entities.Students;

namespace SchoolManagement.Core.Interfaces
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Enrollment>> GetByCourseAsync(string course);
        Task<IEnumerable<Enrollment>> GetByStatusAsync(string status);
    }
}