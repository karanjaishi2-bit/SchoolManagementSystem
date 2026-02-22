namespace SchoolManagement.Core.DTOs.Classes
{
	public class CreateClassRequest
	{
		public string Name { get; set; } = string.Empty;
		public string TeacherName { get; set; } = string.Empty;
		public int StudentCount { get; set; } = 0;
		public string Status { get; set; } = "Active"; // Default to Active
	}
}
