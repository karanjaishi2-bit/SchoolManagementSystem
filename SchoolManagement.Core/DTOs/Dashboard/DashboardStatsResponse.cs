namespace SchoolManagement.Core.DTOs.Dashboard
{
    public class DashboardStatsResponse
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TodayAttendance { get; set; }
        public decimal PendingFees { get; set; }
    }

    public class MonthlyEnrollment
    {
        public string Month { get; set; } = string.Empty;
        public int Enrollments { get; set; }
    }

    public class AttendanceOverview
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Leave { get; set; }
    }
}