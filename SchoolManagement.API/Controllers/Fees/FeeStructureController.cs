using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.DTOs.Fees;
using SchoolManagement.Core.Entities.Fees;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Infrastructure.Data;

namespace SchoolManagement.API.Controllers.Fees
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeeStructureController : ControllerBase
    {
        private readonly IFeeStructureRepository _feeStructureRepository;
        private readonly ApplicationDbContext _context;

        public FeeStructureController(IFeeStructureRepository feeStructureRepository, ApplicationDbContext context)
        {
            _feeStructureRepository = feeStructureRepository;
            _context = context;
        }

        // GET: api/feestructure
        [HttpGet]
        public async Task<ActionResult> GetAllFeeStructures(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? classId = null,
            [FromQuery] string? status = null)
        {
            try
            {
                IEnumerable<FeeStructure> feeStructures;

                if (!string.IsNullOrEmpty(classId))
                {
                    feeStructures = await _feeStructureRepository.GetByClassIdAsync(classId);
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    feeStructures = await _feeStructureRepository.GetByStatusAsync(status);
                }
                else
                {
                    feeStructures = await _feeStructureRepository.GetPagedAsync(page, limit);
                }

                var total = await _feeStructureRepository.GetTotalCountAsync();

                return Ok(new
                {
                    success = true,
                    data = feeStructures,
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

        // GET: api/feestructure/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFeeStructure(int id)
        {
            try
            {
                var feeStructure = await _feeStructureRepository.GetByIdAsync(id);
                if (feeStructure == null)
                {
                    return NotFound(new { success = false, error = "Fee structure not found" });
                }
                return Ok(new { success = true, data = feeStructure });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // GET: api/feestructure/class/10A
        [HttpGet("class/{classId}")]
        public async Task<ActionResult> GetFeeStructureByClass(string classId)
        {
            try
            {
                var feeStructures = await _feeStructureRepository.GetByClassIdAsync(classId);
                return Ok(new { success = true, data = feeStructures });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // POST: api/feestructure
        [HttpPost]
        public async Task<ActionResult> CreateFeeStructure([FromBody] CreateFeeStructureRequest request)
        {
            try
            {
                var feeStructure = new FeeStructure
                {
                    ClassId = request.ClassId,
                    ClassName = request.ClassName,
                    TotalAmount = request.TotalAmount,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdFeeStructure = await _feeStructureRepository.AddAsync(feeStructure);

                // Add fee items
                foreach (var item in request.RecurringItems)
                {
                    var feeItem = new FeeItem
                    {
                        FeeHead = item.FeeHead,
                        Amount = item.Amount,
                        FeeType = "Recurring",
                        Frequency = item.Frequency,
                        Description = item.Description,
                        FeeStructureId = createdFeeStructure.Id,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _context.FeeItems.Add(feeItem);
                }

                foreach (var item in request.OneTimeItems)
                {
                    var feeItem = new FeeItem
                    {
                        FeeHead = item.FeeHead,
                        Amount = item.Amount,
                        FeeType = "One-Time",
                        Frequency = item.Frequency,
                        Description = item.Description,
                        FeeStructureId = createdFeeStructure.Id,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _context.FeeItems.Add(feeItem);
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = createdFeeStructure, message = "Fee structure created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // PUT: api/feestructure/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeeStructure(int id, [FromBody] CreateFeeStructureRequest request)
        {
            try
            {
                var feeStructure = await _feeStructureRepository.GetByIdAsync(id);
                if (feeStructure == null)
                {
                    return NotFound(new { success = false, error = "Fee structure not found" });
                }

                feeStructure.ClassId = request.ClassId;
                feeStructure.ClassName = request.ClassName;
                feeStructure.TotalAmount = request.TotalAmount;
                feeStructure.Status = request.Status;
                feeStructure.UpdatedAt = DateTime.UtcNow;

                await _feeStructureRepository.UpdateAsync(feeStructure);

                return Ok(new { success = true, data = feeStructure, message = "Fee structure updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        // DELETE: api/feestructure/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeeStructure(int id)
        {
            try
            {
                var feeStructure = await _feeStructureRepository.GetByIdAsync(id);
                if (feeStructure == null)
                {
                    return NotFound(new { success = false, error = "Fee structure not found" });
                }

                await _feeStructureRepository.DeleteAsync(id);

                return Ok(new { success = true, message = "Fee structure deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
    }
}