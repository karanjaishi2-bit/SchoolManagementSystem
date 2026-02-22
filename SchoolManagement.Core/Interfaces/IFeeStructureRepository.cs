using SchoolManagement.Core.Entities.Fees;

namespace SchoolManagement.Core.Interfaces
{
    public interface IFeeStructureRepository : IRepository<FeeStructure>
    {
        Task<IEnumerable<FeeStructure>> GetByClassIdAsync(string classId);
        Task<IEnumerable<FeeStructure>> GetByStatusAsync(string status);
    }
}