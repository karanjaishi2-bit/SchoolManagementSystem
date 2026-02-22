namespace SchoolManagement.Core.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Teacher, Staff
        public int? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
    }
}