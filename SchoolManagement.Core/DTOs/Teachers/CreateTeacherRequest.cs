namespace SchoolManagement.Core.DTOs.Teachers
{
    public class CreateTeacherRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string TeacherId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public string Gender { get; set; } = string.Empty;
        public string? Photo { get; set; }
    }
}