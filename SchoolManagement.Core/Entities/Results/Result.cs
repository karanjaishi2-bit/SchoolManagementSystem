using SchoolManagement.Core.Entities.Students;

namespace SchoolManagement.Core.Entities.Results
{
    public class Result : BaseEntity
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int TotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public string ResultStatus { get; set; } = string.Empty; // Changed from ResultValue to match "Pass" or "Fail"

        // Navigation Properties
        public Student? Student { get; set; }
    }
}