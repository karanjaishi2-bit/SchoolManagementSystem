using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.HR;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.HR;

namespace SchoolManagement.API.Controllers.HR
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollRepository _payrollRepository;

        public PayrollController(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        // GET: api/payroll
        [HttpGet]
        public async Task<ActionResult> GetAllPayroll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? month = null,
            [FromQuery] string? status = null,
            [FromQuery] string? employeeType = null,
            [FromQuery] string? search = null)
        {
            try
            {
                IEnumerable<Payroll> payrolls;

                if (!string.IsNullOrEmpty(search))
                {
                    payrolls = (await _payrollRepository.GetAllAsync())
                        .Where(p => p.EmployeeName.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(month))
                {
                    payrolls = await _payrollRepository.GetByMonthAsync(month);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    payrolls = await _payrollRepository.GetByStatusAsync(status);
                }
                else if (!string.IsNullOrEmpty(employeeType))
                {
                    payrolls = await _payrollRepository.GetByEmployeeTypeAsync(employeeType);
                }
                else
                {
                    payrolls = await _payrollRepository.GetPagedAsync(page, limit);
                }

                var total = await _payrollRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = payrolls,
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
        // GET: api/payroll/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPayroll(int id)
        {
            try
            {
                var payroll = await _payrollRepository.GetByIdAsync(id);
                if (payroll == null)
                {
                    return NotFound(new { success = false, error = "Payroll record not found" });
                }
                return Ok(new { success = true, data = payroll });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/payroll/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult> GetPayrollByEmployee(int employeeId)
        {
            try
            {
                var payrolls = await _payrollRepository.GetByEmployeeIdAsync(employeeId);
                return Ok(new { success = true, data = payrolls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/payroll/month/2024-01
        [HttpGet("month/{month}")]
        public async Task<ActionResult> GetPayrollByMonth(string month)
        {
            try
            {
                var payrolls = await _payrollRepository.GetByMonthAsync(month);
                return Ok(new { success = true, data = payrolls });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/payroll/month/2024-01/total
        [HttpGet("month/{month}/total")]
        public async Task<ActionResult> GetTotalPayrollByMonth(string month)
        {
            try
            {
                var total = await _payrollRepository.GetTotalPayrollByMonthAsync(month);
                return Ok(new { success = true, data = new { month, totalPayroll = total } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/payroll
        [HttpPost]
        public async Task<ActionResult> CreatePayroll([FromBody] CreatePayrollRequest request)
        {
            try
            {
                var payroll = new Payroll
                {
                    EmployeeId = request.EmployeeId,
                    EmployeeName = request.EmployeeName,
                    EmployeeType = request.EmployeeType,
                    Month = request.Month,
                    BasicSalary = request.BasicSalary,
                    Allowances = request.Allowances,
                    Deductions = request.Deductions,
                    NetSalary = request.NetSalary,
                    PaymentDate = request.PaymentDate,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdPayroll = await _payrollRepository.AddAsync(payroll);

                return Ok(new { success = true, data = createdPayroll, message = "Payroll record created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/payroll/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayroll(int id, [FromBody] CreatePayrollRequest request)
        {
            try
            {
                var payroll = await _payrollRepository.GetByIdAsync(id);
                if (payroll == null)
                {
                    return NotFound(new { success = false, error = "Payroll record not found" });
                }

                payroll.EmployeeName = request.EmployeeName;
                payroll.EmployeeType = request.EmployeeType;
                payroll.Month = request.Month;
                payroll.BasicSalary = request.BasicSalary;
                payroll.Allowances = request.Allowances;
                payroll.Deductions = request.Deductions;
                payroll.NetSalary = request.NetSalary;
                payroll.PaymentDate = request.PaymentDate;
                payroll.Status = request.Status;
                payroll.UpdatedAt = DateTime.UtcNow;

                await _payrollRepository.UpdateAsync(payroll);

                return Ok(new { success = true, data = payroll, message = "Payroll updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PATCH: api/payroll/5/mark-paid
        [HttpPatch("{id}/mark-paid")]
        public async Task<IActionResult> MarkAsPaid(int id, [FromBody] MarkPaidRequest request)
        {
            try
            {
                var payroll = await _payrollRepository.GetByIdAsync(id);
                if (payroll == null)
                {
                    return NotFound(new { success = false, error = "Payroll record not found" });
                }

                payroll.Status = "Paid";
                payroll.PaymentDate = request.PaymentDate ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
                payroll.UpdatedAt = DateTime.UtcNow;

                await _payrollRepository.UpdateAsync(payroll);

                return Ok(new { success = true, data = payroll, message = "Payroll marked as paid successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/payroll/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayroll(int id)
        {
            try
            {
                var payroll = await _payrollRepository.GetByIdAsync(id);
                if (payroll == null)
                {
                    return NotFound(new { success = false, error = "Payroll record not found" });
                }

                await _payrollRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Payroll deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    public class MarkPaidRequest
    {
        public string? PaymentDate { get; set; }
    }
}