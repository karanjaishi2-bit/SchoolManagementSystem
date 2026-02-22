namespace SchoolManagement.Core.DTOs.Subjects
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string Status { get; set; } = "Active";
    }
}