using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Core.Interfaces;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                return BadRequest("Bu email zaten kayıtlı.");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            await _userRepository.AddAsync(user);
            return Ok("Kayıt başarılı.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Email veya şifre hatalı.");

            var token = _jwtService.GenerateToken(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            });
        }
    }
}