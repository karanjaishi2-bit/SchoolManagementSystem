using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.HR;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.HR;

namespace SchoolManagement.API.Controllers.HR
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestController(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        // GET: api/leaverequest
        [HttpGet]
        public async Task<ActionResult> GetAllLeaveRequests(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? status = null,
            [FromQuery] string? employeeType = null,
            [FromQuery] string? leaveType = null,
            [FromQuery] string? search = null)
        {
            try
            {
                IEnumerable<LeaveRequest> leaveRequests;

                if (!string.IsNullOrEmpty(search))
                {
                    leaveRequests = (await _leaveRequestRepository.GetAllAsync())
                        .Where(l => l.EmployeeName.Contains(search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(leaveType))
                {
                    leaveRequests = (await _leaveRequestRepository.GetAllAsync())
                        .Where(l => l.LeaveType == leaveType)
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    leaveRequests = await _leaveRequestRepository.GetByStatusAsync(status);
                }
                else if (!string.IsNullOrEmpty(employeeType))
                {
                    leaveRequests = await _leaveRequestRepository.GetByEmployeeTypeAsync(employeeType);
                }
                else
                {
                    leaveRequests = await _leaveRequestRepository.GetPagedAsync(page, limit);
                }

                var total = await _leaveRequestRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = leaveRequests,
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

        // GET: api/leaverequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLeaveRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    return NotFound(new { success = false, error = "Leave request not found" });
                }
                return Ok(new { success = true, data = leaveRequest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/leaverequest/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult> GetLeaveRequestsByEmployee(int employeeId)
        {
            try
            {
                var leaveRequests = await _leaveRequestRepository.GetByEmployeeIdAsync(employeeId);
                return Ok(new { success = true, data = leaveRequests });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/leaverequest/pending
        [HttpGet("pending")]
        public async Task<ActionResult> GetPendingLeaveRequests()
        {
            try
            {
                var leaveRequests = await _leaveRequestRepository.GetPendingRequestsAsync();
                return Ok(new { success = true, data = leaveRequests });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/leaverequest
        [HttpPost]
        public async Task<ActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestRequest request)
        {
            try
            {
                var leaveRequest = new LeaveRequest
                {
                    EmployeeId = request.EmployeeId,
                    EmployeeName = request.EmployeeName,
                    EmployeeType = request.EmployeeType,
                    LeaveType = request.LeaveType,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    TotalDays = request.TotalDays,
                    Reason = request.Reason,
                    Status = request.Status,
                    ApprovedBy = request.ApprovedBy,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdLeaveRequest = await _leaveRequestRepository.AddAsync(leaveRequest);

                return Ok(new { success = true, data = createdLeaveRequest, message = "Leave request created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/leaverequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] CreateLeaveRequestRequest request)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    return NotFound(new { success = false, error = "Leave request not found" });
                }

                leaveRequest.EmployeeName = request.EmployeeName;
                leaveRequest.EmployeeType = request.EmployeeType;
                leaveRequest.LeaveType = request.LeaveType;
                leaveRequest.FromDate = request.FromDate;
                leaveRequest.ToDate = request.ToDate;
                leaveRequest.TotalDays = request.TotalDays;
                leaveRequest.Reason = request.Reason;
                leaveRequest.Status = request.Status;
                leaveRequest.ApprovedBy = request.ApprovedBy;
                leaveRequest.UpdatedAt = DateTime.UtcNow;

                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                return Ok(new { success = true, data = leaveRequest, message = "Leave request updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PATCH: api/leaverequest/5/approve
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> ApproveLeaveRequest(int id, [FromBody] ApproveLeaveRequest request)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    return NotFound(new { success = false, error = "Leave request not found" });
                }

                leaveRequest.Status = "Approved";
                leaveRequest.ApprovedBy = request.ApprovedBy;
                leaveRequest.UpdatedAt = DateTime.UtcNow;

                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                return Ok(new { success = true, data = leaveRequest, message = "Leave request approved successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PATCH: api/leaverequest/5/reject
        [HttpPatch("{id}/reject")]
        public async Task<IActionResult> RejectLeaveRequest(int id, [FromBody] ApproveLeaveRequest request)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    return NotFound(new { success = false, error = "Leave request not found" });
                }

                leaveRequest.Status = "Rejected";
                leaveRequest.ApprovedBy = request.ApprovedBy;
                leaveRequest.UpdatedAt = DateTime.UtcNow;

                await _leaveRequestRepository.UpdateAsync(leaveRequest);

                return Ok(new { success = true, data = leaveRequest, message = "Leave request rejected" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/leaverequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
                if (leaveRequest == null)
                {
                    return NotFound(new { success = false, error = "Leave request not found" });
                }

                await _leaveRequestRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Leave request deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }

    public class ApproveLeaveRequest
    {
        public string ApprovedBy { get; set; } = string.Empty;
    }
}