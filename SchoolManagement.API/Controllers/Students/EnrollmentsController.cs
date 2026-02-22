using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Core.Entities.Students;
using SchoolManagement.Core.Interfaces;
using SchoolManagement.Core.DTOs.Students;

namespace SchoolManagement.API.Controllers.Students
{
	[ApiController]
	[Route("api/[controller]")]
	public class EnrollmentsController : ControllerBase
	{
		private readonly IEnrollmentRepository _enrollmentRepository;

		public EnrollmentsController(IEnrollmentRepository enrollmentRepository)
		{
			_enrollmentRepository = enrollmentRepository;
		}

		// GET: api/enrollments
		[HttpGet]
		public async Task<ActionResult> GetAllEnrollments(
			[FromQuery] int page = 1,
			[FromQuery] int limit = 10,
			[FromQuery] string? status = null,
			[FromQuery] string? course = null)
		{
			try
			{
				IEnumerable<Enrollment> enrollments;

				if (!string.IsNullOrEmpty(status))
				{
					enrollments = await _enrollmentRepository.GetByStatusAsync(status);
				}
				else if (!string.IsNullOrEmpty(course))
				{
					enrollments = await _enrollmentRepository.GetByCourseAsync(course);
				}
				else
				{
					enrollments = await _enrollmentRepository.GetPagedAsync(page, limit);
				}

				var total = await _enrollmentRepository.GetTotalCountAsync();

				return Ok(new
				{
					success = true,
					data = enrollments,
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

		// GET: api/enrollments/5
		[HttpGet("{id}")]
		public async Task<ActionResult> GetEnrollment(int id)
		{
			try
			{
				var enrollment = await _enrollmentRepository.GetByIdAsync(id);
				if (enrollment == null)
				{
					return NotFound(new { success = false, error = "Enrollment not found" });
				}
				return Ok(new { success = true, data = enrollment });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}

		// GET: api/enrollments/student/5
		[HttpGet("student/{studentId}")]
		public async Task<ActionResult> GetEnrollmentsByStudent(int studentId)
		{
			try
			{
				var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
				return Ok(new { success = true, data = enrollments });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}

		// POST: api/enrollments
		[HttpPost]
		public async Task<ActionResult> CreateEnrollment([FromBody] CreateEnrollmentRequest request)
		{
			try
			{
				var enrollment = new Enrollment
				{
					StudentId = request.StudentId,
					StudentName = request.StudentName,
					RollNo = request.RollNo,
					Course = request.Course,
					EnrolledOn = request.EnrolledOn,
					Status = request.Status,
					CreatedAt = DateTime.UtcNow,
					IsActive = true
				};

				var createdEnrollment = await _enrollmentRepository.AddAsync(enrollment);

				return Ok(new { success = true, data = createdEnrollment, message = "Enrollment created successfully" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}

		// PUT: api/enrollments/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] CreateEnrollmentRequest request)
		{
			try
			{
				var enrollment = await _enrollmentRepository.GetByIdAsync(id);
				if (enrollment == null)
				{
					return NotFound(new { success = false, error = "Enrollment not found" });
				}

				enrollment.StudentName = request.StudentName;
				enrollment.RollNo = request.RollNo;
				enrollment.Course = request.Course;
				enrollment.EnrolledOn = request.EnrolledOn;
				enrollment.Status = request.Status;
				enrollment.UpdatedAt = DateTime.UtcNow;

				await _enrollmentRepository.UpdateAsync(enrollment);

				return Ok(new { success = true, data = enrollment, message = "Enrollment updated successfully" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}

		// DELETE: api/enrollments/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEnrollment(int id)
		{
			try
			{
				var enrollment = await _enrollmentRepository.GetByIdAsync(id);
				if (enrollment == null)
				{
					return NotFound(new { success = false, error = "Enrollment not found" });
				}

				await _enrollmentRepository.DeleteAsync(id);

				return Ok(new { success = true, message = "Enrollment deleted successfully" });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, error = ex.Message });
			}
		}
	}
}