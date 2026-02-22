using SchoolManagement.Core.Entities.Fees;

namespace SchoolManagement.Core.Interfaces
{
    public interface IFeeRepository : IRepository<Fee>
    {
        Task<IEnumerable<Fee>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Fee>> GetByStatusAsync(string status);
        Task<IEnumerable<Fee>> GetOverdueFeesAsync();
        Task<decimal> GetTotalPendingAmountAsync();
    }
}