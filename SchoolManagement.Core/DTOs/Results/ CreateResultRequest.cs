namespace SchoolManagement.Core.DTOs.Results
{
    public class CreateResultRequest
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int TotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public string Result { get; set; } = string.Empty; // "Pass" or "Fail"
    }
}