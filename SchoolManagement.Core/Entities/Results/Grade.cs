using SchoolManagement.Core.Entities.Students;

namespace SchoolManagement.Core.Entities.Results
{
    public class Grade : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Marks { get; set; }
        public string GradeValue { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;

        // Navigation Properties
        public Student? Student { get; set; }
    }
}