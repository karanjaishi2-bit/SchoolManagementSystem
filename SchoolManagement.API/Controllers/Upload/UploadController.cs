using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement.API.Controllers.Upload
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UploadController> _logger;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public UploadController(IWebHostEnvironment environment, ILogger<UploadController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        // POST: api/upload/photo
        [HttpPost("photo")]
        [RequestSizeLimit(MaxFileSize)]
        public async Task<ActionResult> UploadPhoto([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { success = false, error = "No file uploaded" });
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return BadRequest(new { success = false, error = $"Invalid file type. Allowed: {string.Join(", ", AllowedExtensions)}" });
                }

                if (file.Length > MaxFileSize)
                {
                    return BadRequest(new { success = false, error = "File size must be less than 5MB" });
                }

                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "photos");
                Directory.CreateDirectory(uploadsPath);

                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var uniqueFileName = $"{timestamp}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var photoUrl = $"/uploads/photos/{uniqueFileName}";

                _logger.LogInformation($"Photo uploaded successfully: {uniqueFileName}");

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        url = photoUrl,
                        fileName = uniqueFileName,
                        fileSize = file.Length,
                        uploadedAt = DateTime.UtcNow
                    },
                    message = "Photo uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading photo");
                return StatusCode(500, new { success = false, error = "Failed to upload photo" });
            }
        }

        // DELETE: api/upload/photo
        [HttpDelete("photo")]
        public IActionResult DeletePhoto([FromQuery] string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest(new { success = false, error = "Filename is required" });
                }

                var filePath = Path.Combine(_environment.WebRootPath, "uploads", "photos", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogInformation($"Photo deleted: {fileName}");
                    return Ok(new { success = true, message = "Photo deleted successfully" });
                }

                return NotFound(new { success = false, error = "File not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting photo: {fileName}");
                return StatusCode(500, new { success = false, error = "Failed to delete photo" });
            }
        }
    }
}