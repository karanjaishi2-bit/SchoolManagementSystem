namespace SchoolManagement.Core.Entities.Auth
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Teacher, Staff
        public int? ReferenceId { get; set; } // Links to Teacher.Id or Staff.Id
        public string? ReferenceType { get; set; } // "Teacher" or "Staff"
        public bool IsEmailVerified { get; set; } = false;
        public DateTime? LastLogin { get; set; }
    }
}