using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Subjects;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Subjects;

namespace SchoolManagement.API.Controllers.Subjects
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectsController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        // GET: api/subjects
        [HttpGet]
        public async Task<ActionResult> GetAllSubjects(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null,
            [FromQuery] int? teacherId = null,
            [FromQuery] string? classId = null)
        {
            try
            {
                IEnumerable<Subject> subjects;

                if (teacherId.HasValue)
                {
                    subjects = await _subjectRepository.GetByTeacherIdAsync(teacherId.Value);
                }
                else if (!string.IsNullOrEmpty(classId))
                {
                    subjects = await _subjectRepository.GetByClassIdAsync(classId);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    subjects = await _subjectRepository.GetByStatusAsync(status);
                }
                else
                {
                    subjects = await _subjectRepository.GetPagedAsync(page, limit);
                }

                var total = await _subjectRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = subjects,
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

        // GET: api/subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSubject(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return NotFound(new { success = false, error = "Subject not found" });
                }
                return Ok(new { success = true, data = subject });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/subjects/code/MATH101
        [HttpGet("code/{code}")]
        public async Task<ActionResult> GetSubjectByCode(string code)
        {
            try
            {
                var subject = await _subjectRepository.GetByCodeAsync(code);
                if (subject == null)
                {
                    return NotFound(new { success = false, error = "Subject not found" });
                }
                return Ok(new { success = true, data = subject });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/subjects/teacher/5
        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult> GetSubjectsByTeacher(int teacherId)
        {
            try
            {
                var subjects = await _subjectRepository.GetByTeacherIdAsync(teacherId);
                return Ok(new { success = true, data = subjects });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/subjects/class/10A
        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetSubjectsByClass(string classId)
        {
            try
            {
                var subjects = await _subjectRepository.GetByClassIdAsync(classId);
                return Ok(new { success = true, data = subjects });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/subjects
        [HttpPost]
        public async Task<ActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            try
            {
                var existingSubject = await _subjectRepository.GetByCodeAsync(request.Code);
                if (existingSubject != null)
                {
                    return BadRequest(new { success = false, error = "Subject code already exists" });
                }

                var subject = new Subject
                {
                    Name = request.Name,
                    Code = request.Code,
                    TeacherId = request.TeacherId,
                    TeacherName = request.TeacherName,
                    ClassId = request.ClassId,
                    ClassName = request.ClassName,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdSubject = await _subjectRepository.AddAsync(subject);

                return Ok(new { success = true, data = createdSubject, message = "Subject created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/subjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] CreateSubjectRequest request)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return NotFound(new { success = false, error = "Subject not found" });
                }

                subject.Name = request.Name;
                subject.Code = request.Code;
                subject.TeacherId = request.TeacherId;
                subject.TeacherName = request.TeacherName;
                subject.ClassId = request.ClassId;
                subject.ClassName = request.ClassName;
                subject.Status = request.Status;
                subject.UpdatedAt = DateTime.UtcNow;

                await _subjectRepository.UpdateAsync(subject);

                return Ok(new { success = true, data = subject, message = "Subject updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                var subject = await _subjectRepository.GetByIdAsync(id);
                if (subject == null)
                {
                    return NotFound(new { success = false, error = "Subject not found" });
                }

                await _subjectRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Subject deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}