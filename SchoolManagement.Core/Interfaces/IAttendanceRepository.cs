using SchoolManagement.Core.Entities.Attendance;

namespace SchoolManagement.Core.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetByEntityIdAsync(int entityId);
        Task<IEnumerable<Attendance>> GetByEntityTypeAsync(string entityType);
        Task<IEnumerable<Attendance>> GetByDateAsync(string date);
        Task<IEnumerable<Attendance>> GetByDateRangeAsync(string startDate, string endDate);
        Task<IEnumerable<Attendance>> GetByStatusAsync(string status);
        Task<IEnumerable<Attendance>> GetByClassIdAsync(string classId);
    }
}