using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Fees;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Fees;
namespace SchoolManagement.API.Controllers.Fees
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFeeRepository _feeRepository;

        public FeesController(IFeeRepository feeRepository)
        {
            _feeRepository = feeRepository;
        }

        // GET: api/fees
        [HttpGet]
        public async Task<ActionResult> GetAllFees(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null,
            [FromQuery] int? studentId = null,
            [FromQuery] string? classId = null,
            [FromQuery] string? startDate = null,
            [FromQuery] string? endDate = null)
        {
            try
            {
                IEnumerable<Fee> fees;

                if (studentId.HasValue)
                {
                    fees = await _feeRepository.GetByStudentIdAsync(studentId.Value);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    fees = await _feeRepository.GetByStatusAsync(status);
                }
                else
                {
                    fees = await _feeRepository.GetPagedAsync(page, limit);
                }

                // Apply additional filters
                if (!string.IsNullOrEmpty(classId))
                {
                    fees = fees.Where(f => f.ClassId == classId);
                }

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    fees = fees.Where(f =>
                        string.Compare(f.DueDate, startDate) >= 0 &&
                        string.Compare(f.DueDate, endDate) <= 0
                    );
                }

                var total = await _feeRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = fees,
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

        // GET: api/fees/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFee(int id)
        {
            try
            {
                var fee = await _feeRepository.GetByIdAsync(id);
                if (fee == null)
                {
                    return NotFound(new { success = false, error = "Fee record not found" });
                }
                return Ok(new { success = true, data = fee });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/fees/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult> GetFeesByStudent(int studentId)
        {
            try
            {
                var fees = await _feeRepository.GetByStudentIdAsync(studentId);
                return Ok(new { success = true, data = fees });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/fees/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult> GetOverdueFees()
        {
            try
            {
                var fees = await _feeRepository.GetOverdueFeesAsync();
                return Ok(new { success = true, data = fees });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/fees/pending/total
        [HttpGet("pending/total")]
        public async Task<ActionResult> GetTotalPendingAmount()
        {
            try
            {
                var total = await _feeRepository.GetTotalPendingAmountAsync();
                return Ok(new { success = true, data = new { totalPending = total } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/fees
        [HttpPost]
        public async Task<ActionResult> CreateFee([FromBody] CreateFeeRequest request)
        {
            try
            {
                var fee = new Fee
                {
                    StudentName = request.StudentName,
                    StudentId = request.StudentId,
                    Amount = request.Amount,
                    DueDate = request.DueDate,
                    Status = request.Status,
                    ClassId = request.ClassId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdFee = await _feeRepository.AddAsync(fee);

                return Ok(new { success = true, data = createdFee, message = "Fee record created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/fees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] CreateFeeRequest request)
        {
            try
            {
                var fee = await _feeRepository.GetByIdAsync(id);
                if (fee == null)
                {
                    return NotFound(new { success = false, error = "Fee record not found" });
                }

                fee.StudentName = request.StudentName;
                fee.StudentId = request.StudentId;
                fee.Amount = request.Amount;
                fee.DueDate = request.DueDate;
                fee.Status = request.Status;
                fee.ClassId = request.ClassId;
                fee.UpdatedAt = DateTime.UtcNow;

                await _feeRepository.UpdateAsync(fee);

                return Ok(new { success = true, data = fee, message = "Fee updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PATCH: api/fees/5/pay
        [HttpPatch("{id}/pay")]
        public async Task<IActionResult> MarkFeeAsPaid(int id)
        {
            try
            {
                var fee = await _feeRepository.GetByIdAsync(id);
                if (fee == null)
                {
                    return NotFound(new { success = false, error = "Fee record not found" });
                }

                fee.Status = "Paid";
                fee.UpdatedAt = DateTime.UtcNow;

                await _feeRepository.UpdateAsync(fee);

                return Ok(new { success = true, data = fee, message = "Fee marked as paid successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/fees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {
            try
            {
                var fee = await _feeRepository.GetByIdAsync(id);
                if (fee == null)
                {
                    return NotFound(new { success = false, error = "Fee record not found" });
                }

                await _feeRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Fee deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
} 