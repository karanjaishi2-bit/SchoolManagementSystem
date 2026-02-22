using SchoolManagement.Core.Entities.Classes;

namespace SchoolManagement.Core.Interfaces
{
    public interface IClassRepository : IRepository<ClassData>
    {
        Task<ClassData?> GetByNameAsync(string name);
        Task<IEnumerable<ClassData>> GetByStatusAsync(string status);
    }
}