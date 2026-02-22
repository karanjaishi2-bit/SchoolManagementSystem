using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Results;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Results;

namespace SchoolManagement.API.Controllers.Results
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepository;

        public GradesController(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        // GET: api/grades
        [HttpGet]
        public async Task<ActionResult> GetAllGrades(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            try
            {
                var grades = await _gradeRepository.GetPagedAsync(page, limit);
                var total = await _gradeRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = grades,
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

        // GET: api/grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetGrade(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    return NotFound(new { success = false, error = "Grade not found" });
                }
                return Ok(new { success = true, data = grade });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/grades/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult> GetGradesByStudent(int studentId)
        {
            try
            {
                var grades = await _gradeRepository.GetByStudentIdAsync(studentId);
                return Ok(new { success = true, data = grades });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/grades
        [HttpPost]
        public async Task<ActionResult> CreateGrade([FromBody] CreateGradeRequest request)
        {
            try
            {
                var grade = new Grade
                {
                    StudentId = request.StudentId,
                    StudentName = request.StudentName,
                    Subject = request.Subject,
                    Marks = request.Marks,
                    GradeValue = request.GradeValue,
                    Result = request.Result,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdGrade = await _gradeRepository.AddAsync(grade);

                return Ok(new { success = true, data = createdGrade, message = "Grade created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/grades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] CreateGradeRequest request)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    return NotFound(new { success = false, error = "Grade not found" });
                }

                grade.StudentName = request.StudentName;
                grade.Subject = request.Subject;
                grade.Marks = request.Marks;
                grade.GradeValue = request.GradeValue;
                grade.Result = request.Result;
                grade.UpdatedAt = DateTime.UtcNow;

                await _gradeRepository.UpdateAsync(grade);

                return Ok(new { success = true, data = grade, message = "Grade updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/grades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(id);
                if (grade == null)
                {
                    return NotFound(new { success = false, error = "Grade not found" });
                }

                await _gradeRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Grade deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}