namespace SchoolManagement.Core.Entities.Classes
{
    public class ClassData : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public string Status { get; set; } = "Active"; // Active, Inactive
    }
}