using SchoolManagement.Core.Enums;

namespace SchoolManagement.Core.DTOs.Students
{
    public class CreateStudentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int RollNo { get; set; }
        public string Status { get; set; } = EntityStatus.Active;
        public string Gender { get; set; } = Enums.Gender.Male; // ✅ FIXED
        public string? Photo { get; set; }
    }
}
