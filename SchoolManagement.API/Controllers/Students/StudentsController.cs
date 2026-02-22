using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Students;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Students;

namespace SchoolManagement.API.Controllers.Students
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        // GET: api/students
        [HttpGet]
        public async Task<ActionResult> GetAllStudents(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? className = null,
            [FromQuery] string? section = null,
            [FromQuery] string? gender = null,
            [FromQuery] string? search = null)
        {
            try
            {
                IEnumerable<Student> students;

                // Search has highest priority
                if (!string.IsNullOrEmpty(search))
                {
                    students = await _studentRepository.SearchAsync(search);
                }
                // Then class filter
                else if (!string.IsNullOrEmpty(className))
                {
                    students = await _studentRepository.GetByClassAsync(className);
                }
                // Then status filter
                else if (!string.IsNullOrEmpty(status))
                {
                    students = await _studentRepository.GetByStatusAsync(status);
                }
                // Default: get all paginated
                else
                {
                    students = await _studentRepository.GetPagedAsync(page, limit);
                }

                // Apply additional filters on the result
                if (!string.IsNullOrEmpty(section))
                {
                    students = students.Where(s => s.Section == section);
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    students = students.Where(s => s.Gender == gender);
                }

                var total = await _studentRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = students,
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

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                {
                    return NotFound(new { success = false, error = "Student not found" });
                }

                return Ok(new { success = true, data = student });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] CreateStudentRequest request)
        {
            try
            {
                var existingStudent = await _studentRepository.GetByStudentIdAsync(request.StudentId);
                if (existingStudent != null)
                {
                    return BadRequest(new { success = false, error = "Student ID already exists" });
                }

                var existingEmail = await _studentRepository.GetByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    return BadRequest(new { success = false, error = "Email already exists" });
                }

                var student = new Student
                {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                    Dob = request.Dob,
                    StudentId = request.StudentId,
                    Class = request.Class,
                    RollNo = request.RollNo,
                    Status = request.Status,
                    Gender = request.Gender,
                    Photo = request.Photo,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdStudent = await _studentRepository.AddAsync(student);

                return Ok(new { success = true, data = createdStudent, message = "Student created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] CreateStudentRequest request)
        {
            try
            {
                var existingStudent = await _studentRepository.GetByIdAsync(id);
                if (existingStudent == null)
                {
                    return NotFound(new { success = false, error = "Student not found" });
                }

                existingStudent.Name = request.Name;
                existingStudent.Email = request.Email;
                existingStudent.Phone = request.Phone;
                existingStudent.Dob = request.Dob;
                existingStudent.Class = request.Class;
                existingStudent.RollNo = request.RollNo;
                existingStudent.Status = request.Status;
                existingStudent.UpdatedAt = DateTime.UtcNow;
                existingStudent.Gender = request.Gender;
                existingStudent.Photo = request.Photo;
                await _studentRepository.UpdateAsync(existingStudent);

                return Ok(new { success = true, data = existingStudent, message = "Student updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { success = false, error = "Student not found" });
                }

                await _studentRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Student deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}