using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.DTOs.Fees;
using SchoolManagement.Core.Entities.Fees;
using SchoolManagement.Core.Interfaces;

namespace SchoolManagement.API.Controllers.Fees
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeBillController : ControllerBase
    {
        private readonly IFeeBillRepository _feeBillRepository;
        private readonly IFeeRepository _feeRepository;

        public FeeBillController(
            IFeeBillRepository feeBillRepository,
            IFeeRepository feeRepository)
        {
            _feeBillRepository = feeBillRepository;
            _feeRepository = feeRepository;
        }

        // GET: api/feebill
        [HttpGet]
        public async Task<ActionResult> GetAllFeeBills(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] int? studentId = null,
            [FromQuery] string? classId = null,
            [FromQuery] string? status = null)
        {
            try
            {
                IEnumerable<FeeBill> feeBills;

                if (studentId.HasValue)
                {
                    feeBills = await _feeBillRepository.GetByStudentIdAsync(studentId.Value);
                }
                else if (!string.IsNullOrEmpty(classId))
                {
                    feeBills = await _feeBillRepository.GetByClassIdAsync(classId);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    feeBills = await _feeBillRepository.GetByStatusAsync(status);
                }
                else
                {
                    feeBills = await _feeBillRepository.GetPagedAsync(page, limit);
                }

                var total = await _feeBillRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = feeBills,
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

        // GET: api/feebill/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFeeBill(int id)
        {
            try
            {
                var feeBill = await _feeBillRepository.GetByIdAsync(id);
                if (feeBill == null)
                {
                    return NotFound(new { success = false, error = "Fee bill not found" });
                }

                return Ok(new { success = true, data = feeBill });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/feebill/student/5
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult> GetFeeBillsByStudent(int studentId)
        {
            try
            {
                var feeBills = await _feeBillRepository.GetByStudentIdAsync(studentId);
                return Ok(new { success = true, data = feeBills });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/feebill/class/10A
        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetFeeBillsByClass(string classId)
        {
            try
            {
                var feeBills = await _feeBillRepository.GetByClassIdAsync(classId);
                return Ok(new { success = true, data = feeBills });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/feebill/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult> GetOverdueFeeBills()
        {
            try
            {
                var feeBills = await _feeBillRepository.GetOverdueBillsAsync();
                return Ok(new { success = true, data = feeBills });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/feebill
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

        // PUT: api/feebill/fees/5
        [HttpPut("fees/{id}")]
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
                fee.UpdatedAt = DateTime.UtcNow;

                await _feeRepository.UpdateAsync(fee);

                return Ok(new { success = true, data = fee, message = "Fee updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/feebill/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeeBill(int id, [FromBody] CreateFeeBillRequest request)
        {
            try
            {
                var feeBill = await _feeBillRepository.GetByIdAsync(id);
                if (feeBill == null)
                {
                    return NotFound(new { success = false, error = "Fee bill not found" });
                }

                feeBill.StudentName = request.StudentName;
                feeBill.ClassId = request.ClassId;
                feeBill.ClassName = request.ClassName;
                feeBill.BillDate = request.BillDate;
                feeBill.DueDate = request.DueDate;
                feeBill.TotalAmount = request.TotalAmount;
                feeBill.PaidAmount = request.PaidAmount;
                feeBill.BalanceAmount = request.BalanceAmount;
                feeBill.Status = request.Status;
                feeBill.UpdatedAt = DateTime.UtcNow;

                await _feeBillRepository.UpdateAsync(feeBill);

                return Ok(new { success = true, data = feeBill, message = "Fee bill updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PATCH: api/feebill/5/pay
        [HttpPatch("{id}/pay")]
        public async Task<IActionResult> RecordPayment(int id, [FromBody] PaymentRequest request)
        {
            try
            {
                var feeBill = await _feeBillRepository.GetByIdAsync(id);
                if (feeBill == null)
                {
                    return NotFound(new { success = false, error = "Fee bill not found" });
                }

                feeBill.PaidAmount += request.Amount;
                feeBill.BalanceAmount = feeBill.TotalAmount - feeBill.PaidAmount;

                if (feeBill.BalanceAmount <= 0)
                {
                    feeBill.Status = "Paid";
                }
                else if (feeBill.PaidAmount > 0)
                {
                    feeBill.Status = "Partial";
                }

                feeBill.UpdatedAt = DateTime.UtcNow;

                await _feeBillRepository.UpdateAsync(feeBill);

                return Ok(new { success = true, data = feeBill, message = "Payment recorded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/feebill/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeeBill(int id)
        {
            try
            {
                var feeBill = await _feeBillRepository.GetByIdAsync(id);
                if (feeBill == null)
                {
                    return NotFound(new { success = false, error = "Fee bill not found" });
                }

                await _feeBillRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Fee bill deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}