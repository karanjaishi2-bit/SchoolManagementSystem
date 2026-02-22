using SchoolManagement.Core.Entities.Classes;

namespace SchoolManagement.Core.Entities.Students
{
    public class Enrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string EnrolledOn { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";

        // Navigation Properties
        public Student? Student { get; set; }
    }
}