using SchoolManagement.Core.Entities.Results;

namespace SchoolManagement.Core.Interfaces
{
    public interface IResultRepository : IRepository<Result>
    {
        Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Result>> GetByClassAsync(string className);
    }
}