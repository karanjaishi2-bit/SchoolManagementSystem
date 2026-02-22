namespace SchoolManagement.Core.DTOs.Students
{
    public class CreateEnrollmentRequest
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string EnrolledOn { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }
}