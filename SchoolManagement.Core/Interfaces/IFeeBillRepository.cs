using SchoolManagement.Core.Entities.Fees;

namespace SchoolManagement.Core.Interfaces
{
    public interface IFeeBillRepository : IRepository<FeeBill>
    {
        Task<IEnumerable<FeeBill>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<FeeBill>> GetByClassIdAsync(string classId);
        Task<IEnumerable<FeeBill>> GetByStatusAsync(string status);
        Task<IEnumerable<FeeBill>> GetOverdueBillsAsync();
    }
}