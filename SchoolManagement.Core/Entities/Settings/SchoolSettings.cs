namespace SchoolManagement.Core.Entities.Settings
{
    public class SchoolSettings : BaseEntity
    {
        public string SchoolName { get; set; } = string.Empty;
        public string SchoolCode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
        public string CurrentAcademicYear { get; set; } = string.Empty;
        public string AcademicYearStartDate { get; set; } = string.Empty;
        public string AcademicYearEndDate { get; set; } = string.Empty;
    }
}