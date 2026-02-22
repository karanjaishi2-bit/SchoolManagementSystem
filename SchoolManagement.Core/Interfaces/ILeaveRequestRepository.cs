using SchoolManagement.Core.Entities.HR;

namespace SchoolManagement.Core.Interfaces
{
    public interface ILeaveRequestRepository : IRepository<LeaveRequest>
    {
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeTypeAsync(string employeeType);
        Task<IEnumerable<LeaveRequest>> GetByStatusAsync(string status);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync();
    }
}