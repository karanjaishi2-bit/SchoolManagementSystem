namespace SchoolManagement.Core.DTOs.Attendance
{
    public class CreateAttendanceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty; // Student, Teacher, Staff
        public int? EntityId { get; set; }
        public string? ClassId { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Present, Absent, Leave
    }
}