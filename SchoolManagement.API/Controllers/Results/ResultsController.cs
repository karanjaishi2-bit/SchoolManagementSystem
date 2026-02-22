using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Results;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Results;


namespace SchoolManagement.API.Controllers.Results
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;

        public ResultsController(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        // GET: api/results
        [HttpGet]
        public async Task<ActionResult> GetAllResults(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            try
            {
                var results = await _resultRepository.GetPagedAsync(page, limit);
                var total = await _resultRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = results,
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

        // GET: api/results/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetResult(int id)
        {
            try
            {
                var result = await _resultRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, error = "Result not found" });
                }
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/results/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult> GetResultsByStudent(int studentId)
        {
            try
            {
                var results = await _resultRepository.GetByStudentIdAsync(studentId);
                return Ok(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/results/class/10A
        [HttpGet("class/{className}")]
        public async Task<ActionResult> GetResultsByClass(string className)
        {
            try
            {
                var results = await _resultRepository.GetByClassAsync(className);
                return Ok(new { success = true, data = results });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/results
        [HttpPost]
        public async Task<ActionResult> CreateResult([FromBody] CreateResultRequest request)
        {
            try
            {
                var result = new Result
                {
                    StudentId = request.StudentId,
                    StudentName = request.StudentName,
                    ClassName = request.ClassName,
                    TotalMarks = request.TotalMarks,
                    Percentage = request.Percentage,
                    ResultStatus = request.Result,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdResult = await _resultRepository.AddAsync(result);

                return Ok(new { success = true, data = createdResult, message = "Result created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/results/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, [FromBody] CreateResultRequest request)
        {
            try
            {
                var result = await _resultRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, error = "Result not found" });
                }

                result.StudentName = request.StudentName;
                result.ClassName = request.ClassName;
                result.TotalMarks = request.TotalMarks;
                result.Percentage = request.Percentage;
                result.ResultStatus = request.Result;
                result.UpdatedAt = DateTime.UtcNow;

                await _resultRepository.UpdateAsync(result);

                return Ok(new { success = true, data = result, message = "Result updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            try
            {
                var result = await _resultRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, error = "Result not found" });
                }

                await _resultRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Result deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}