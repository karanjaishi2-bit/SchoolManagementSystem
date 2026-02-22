using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Attendance;

namespace SchoolManagement.API.Controllers.Attendance
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        // GET: api/attendance
        // GET: api/attendance
        [HttpGet]
        public async Task<ActionResult> GetAllAttendance(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? entityType = null,
            [FromQuery] int? entityId = null,
            [FromQuery] string? classId = null,
            [FromQuery] string? startDate = null,
            [FromQuery] string? endDate = null)
        {
            try
            {
                IEnumerable<Core.Entities.Attendance.Attendance> attendance;

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    attendance = await _attendanceRepository.GetByDateRangeAsync(startDate, endDate);
                }
                else if (entityId.HasValue)
                {
                    attendance = await _attendanceRepository.GetByEntityIdAsync(entityId.Value);
                }
                else if (!string.IsNullOrEmpty(classId))
                {
                    attendance = await _attendanceRepository.GetByClassIdAsync(classId);
                }
                else if (!string.IsNullOrEmpty(entityType))
                {
                    attendance = await _attendanceRepository.GetByEntityTypeAsync(entityType);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    attendance = await _attendanceRepository.GetByStatusAsync(status);
                }
                else
                {
                    attendance = await _attendanceRepository.GetPagedAsync(page, limit);
                }

                var total = await _attendanceRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = attendance,
                    pagination = new
                    {
                        page,
                        limit,
                        total,
                        pages = (int)Math.Ceiling(total / (double)limit)
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/attendance/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAttendance(int id)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                {
                    return NotFound(new { success = false, error = "Attendance record not found" });
                }
                return Ok(new { success = true, data = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/attendance/entity/5
        [HttpGet("entity/{entityId}")]
        public async Task<ActionResult> GetAttendanceByEntity(int entityId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByEntityIdAsync(entityId);
                return Ok(new { success = true, data = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/attendance/type/Student
        [HttpGet("type/{entityType}")]
        public async Task<ActionResult> GetAttendanceByType(string entityType)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByEntityTypeAsync(entityType);
                return Ok(new { success = true, data = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/attendance/date/2024-01-15
        [HttpGet("date/{date}")]
        public async Task<ActionResult> GetAttendanceByDate(string date)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByDateAsync(date);
                return Ok(new { success = true, data = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/attendance/class/10A
        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetAttendanceByClass(string classId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByClassIdAsync(classId);
                return Ok(new { success = true, data = attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/attendance
        [HttpPost]
        public async Task<ActionResult> CreateAttendance([FromBody] CreateAttendanceRequest request)
        {
            try
            {
                var attendance = new Core.Entities.Attendance.Attendance
                {
                    Name = request.Name,
                    EntityType = request.EntityType,
                    EntityId = request.EntityId,
                    ClassId = request.ClassId,
                    Date = request.Date,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdAttendance = await _attendanceRepository.AddAsync(attendance);

                return Ok(new { success = true, data = createdAttendance, message = "Attendance recorded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/attendance/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult> CreateBulkAttendance([FromBody] List<CreateAttendanceRequest> requests)
        {
            try
            {
                var attendanceList = new List<Core.Entities.Attendance.Attendance>();

                foreach (var request in requests)
                {
                    var attendance = new Core.Entities.Attendance.Attendance
                    {
                        Name = request.Name,
                        EntityType = request.EntityType,
                        EntityId = request.EntityId,
                        ClassId = request.ClassId,
                        Date = request.Date,
                        Status = request.Status,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    await _attendanceRepository.AddAsync(attendance);
                    attendanceList.Add(attendance);
                }

                return Ok(new { success = true, data = attendanceList, message = $"{attendanceList.Count} attendance records created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/attendance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] CreateAttendanceRequest request)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                {
                    return NotFound(new { success = false, error = "Attendance record not found" });
                }

                attendance.Name = request.Name;
                attendance.EntityType = request.EntityType;
                attendance.EntityId = request.EntityId;
                attendance.ClassId = request.ClassId;
                attendance.Date = request.Date;
                attendance.Status = request.Status;
                attendance.UpdatedAt = DateTime.UtcNow;

                await _attendanceRepository.UpdateAsync(attendance);

                return Ok(new { success = true, data = attendance, message = "Attendance updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/attendance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                {
                    return NotFound(new { success = false, error = "Attendance record not found" });
                }

                await _attendanceRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Attendance deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}