using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities.Settings;
using SchoolManagement.Core.DTOs.Settings;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.API.Controllers.Settings
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/settings
        [HttpGet]
        public async Task<ActionResult> GetSettings()
        {
            try
            {
                var settings = await _context.SchoolSettings.FirstOrDefaultAsync();

                if (settings == null)
                {
                    // Return default settings if none exist
                    settings = new SchoolSettings
                    {
                        SchoolName = "School Name",
                        SchoolCode = "SCH001",
                        Address = "",
                        City = "",
                        State = "",
                        PinCode = "",
                        Phone = "",
                        Email = "",
                        CurrentAcademicYear = DateTime.UtcNow.Year.ToString(),
                        AcademicYearStartDate = $"{DateTime.UtcNow.Year}-01-01",
                        AcademicYearEndDate = $"{DateTime.UtcNow.Year}-12-31",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.SchoolSettings.Add(settings);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { success = true, data = settings });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/settings
        [HttpPut]
        public async Task<ActionResult> UpdateSettings([FromBody] UpdateSchoolSettingsRequest request)
        {
            try
            {
                var settings = await _context.SchoolSettings.FirstOrDefaultAsync();

                if (settings == null)
                {
                    // Create new settings
                    settings = new SchoolSettings
                    {
                        SchoolName = request.SchoolName,
                        SchoolCode = request.SchoolCode,
                        Address = request.Address,
                        City = request.City,
                        State = request.State,
                        PinCode = request.PinCode,
                        Phone = request.Phone,
                        Email = request.Email,
                        Website = request.Website,
                        LogoUrl = request.LogoUrl,
                        CurrentAcademicYear = request.CurrentAcademicYear,
                        AcademicYearStartDate = request.AcademicYearStartDate,
                        AcademicYearEndDate = request.AcademicYearEndDate,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.SchoolSettings.Add(settings);
                }
                else
                {
                    // Update existing settings
                    settings.SchoolName = request.SchoolName;
                    settings.SchoolCode = request.SchoolCode;
                    settings.Address = request.Address;
                    settings.City = request.City;
                    settings.State = request.State;
                    settings.PinCode = request.PinCode;
                    settings.Phone = request.Phone;
                    settings.Email = request.Email;
                    settings.Website = request.Website;
                    settings.LogoUrl = request.LogoUrl;
                    settings.CurrentAcademicYear = request.CurrentAcademicYear;
                    settings.AcademicYearStartDate = request.AcademicYearStartDate;
                    settings.AcademicYearEndDate = request.AcademicYearEndDate;
                    settings.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = settings, message = "Settings updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/settings/academic-year
        [HttpGet("academic-year")]
        public async Task<ActionResult> GetAcademicYear()
        {
            try
            {
                var settings = await _context.SchoolSettings.FirstOrDefaultAsync();

                if (settings == null)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new
                        {
                            currentAcademicYear = DateTime.UtcNow.Year.ToString(),
                            startDate = $"{DateTime.UtcNow.Year}-01-01",
                            endDate = $"{DateTime.UtcNow.Year}-12-31"
                        }
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        currentAcademicYear = settings.CurrentAcademicYear,
                        startDate = settings.AcademicYearStartDate,
                        endDate = settings.AcademicYearEndDate
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/settings/academic-year
        [HttpPut("academic-year")]
        public async Task<ActionResult> UpdateAcademicYear([FromBody] UpdateAcademicYearRequest request)
        {
            try
            {
                var settings = await _context.SchoolSettings.FirstOrDefaultAsync();

                if (settings == null)
                {
                    return NotFound(new { success = false, error = "Settings not found. Please initialize settings first." });
                }

                settings.CurrentAcademicYear = request.AcademicYear;
                settings.AcademicYearStartDate = request.StartDate;
                settings.AcademicYearEndDate = request.EndDate;
                settings.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = settings, message = "Academic year updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    public class UpdateAcademicYearRequest
    {
        public string AcademicYear { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
    }
}