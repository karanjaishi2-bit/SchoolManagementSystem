using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Classes;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Classes;



namespace SchoolManagement.API.Controllers.Classes
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepository;

        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        // GET: api/classes
        [HttpGet]
        public async Task<ActionResult> GetAllClasses(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null)
        {
            try
            {
                IEnumerable<ClassData> classes;

                if (!string.IsNullOrEmpty(status))
                {
                    classes = await _classRepository.GetByStatusAsync(status);
                }
                else
                {
                    classes = await _classRepository.GetPagedAsync(page, limit);
                }

                var total = await _classRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = classes,
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

        // GET: api/classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetClass(int id)
        {
            try
            {
                var classData = await _classRepository.GetByIdAsync(id);
                if (classData == null)
                {
                    return NotFound(new { success = false, error = "Class not found" });
                }
                return Ok(new { success = true, data = classData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/classes
        [HttpPost]
        public async Task<ActionResult> CreateClass([FromBody] CreateClassRequest request)
        {
            try
            {
                var existingClass = await _classRepository.GetByNameAsync(request.Name);
                if (existingClass != null)
                {
                    return BadRequest(new { success = false, error = "Class name already exists" });
                }

                var classData = new ClassData
                {
                    Name = request.Name,
                    TeacherName = request.TeacherName,
                    StudentCount = request.StudentCount,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdClass = await _classRepository.AddAsync(classData);

                return Ok(new { success = true, data = createdClass, message = "Class created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/classes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] CreateClassRequest request)
        {
            try
            {
                var classData = await _classRepository.GetByIdAsync(id);
                if (classData == null)
                {
                    return NotFound(new { success = false, error = "Class not found" });
                }

                classData.Name = request.Name;
                classData.TeacherName = request.TeacherName;
                classData.StudentCount = request.StudentCount;
                classData.Status = request.Status;
                classData.UpdatedAt = DateTime.UtcNow;

                await _classRepository.UpdateAsync(classData);

                return Ok(new { success = true, data = classData, message = "Class updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                var classData = await _classRepository.GetByIdAsync(id);
                if (classData == null)
                {
                    return NotFound(new { success = false, error = "Class not found" });
                }

                await _classRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Class deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}