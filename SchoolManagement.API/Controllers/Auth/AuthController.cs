using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchoolManagement.Core.Entities.Auth;
using SchoolManagement.Core.Interfaces;

namespace SchoolManagement.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] SchoolManagement.Core.DTOs.Auth.RegisterRequest request)
        {
            try
            {
                var validRoles = new[] { "Admin", "Teacher", "Staff" };
                if (!validRoles.Contains(request.Role))
                {
                    return BadRequest(new { success = false, error = "Invalid role" });
                }

                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, error = "Email already registered" });
                }

                var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUsername != null)
                {
                    return BadRequest(new { success = false, error = "Username already taken" });
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    Role = request.Role,
                    ReferenceId = request.ReferenceId,
                    ReferenceType = request.ReferenceType,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var createdUser = await _userRepository.AddAsync(user);
                var token = GenerateJwtToken(createdUser);

                var response = new SchoolManagement.Core.DTOs.Auth.AuthResponse
                {
                    Id = createdUser.Id,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    Token = token
                };

                return Ok(new { success = true, data = response, message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] SchoolManagement.Core.DTOs.Auth.LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { success = false, error = "Invalid email or password" });
                }

                if (!user.IsActive)
                {
                    return Unauthorized(new { success = false, error = "Account is inactive" });
                }

                user.LastLogin = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                var token = GenerateJwtToken(user);

                var response = new SchoolManagement.Core.DTOs.Auth.AuthResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token
                };

                return Ok(new { success = true, data = response, message = "Login successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("me")]
        public async Task<ActionResult> GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { success = false, error = "Not authenticated" });
                }

                var user = await _userRepository.GetByIdAsync(int.Parse(userIdClaim));
                if (user == null)
                {
                    return NotFound(new { success = false, error = "User not found" });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = user.Id,
                        username = user.Username,
                        email = user.Email,
                        role = user.Role,
                        referenceId = user.ReferenceId,
                        referenceType = user.ReferenceType
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] string? role = null)
        {
            try
            {
                IEnumerable<User> users;

                if (!string.IsNullOrEmpty(role))
                {
                    users = await _userRepository.GetByRoleAsync(role);
                }
                else
                {
                    users = await _userRepository.GetAllAsync();
                }

                var userList = users.Select(u => new
                {
                    id = u.Id,
                    username = u.Username,
                    email = u.Email,
                    role = u.Role,
                    referenceId = u.ReferenceId,
                    referenceType = u.ReferenceType,
                    isActive = u.IsActive,
                    lastLogin = u.LastLogin,
                    createdAt = u.CreatedAt
                });

                return Ok(new { success = true, data = userList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyForJWTTokenGenerationMinimum32Characters";
            var issuer = jwtSettings["Issuer"] ?? "SchoolManagementAPI";
            var audience = jwtSettings["Audience"] ?? "SchoolManagementClient";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("ReferenceId", user.ReferenceId?.ToString() ?? ""),
                new Claim("ReferenceType", user.ReferenceType ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}