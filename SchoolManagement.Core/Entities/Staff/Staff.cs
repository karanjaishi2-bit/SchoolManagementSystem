namespace SchoolManagement.Core.Entities.Staff
{
    public class Staff : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string StaffId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? Photo { get; set; }
        public string Status { get; set; } = "Active";
    }
}