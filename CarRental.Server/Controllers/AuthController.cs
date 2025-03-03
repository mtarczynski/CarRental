using CarRental.Server.Dto.Auth;
using CarRental.Server.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                var customer = await _authService.RegisterCustomer(dto.Name, dto.Email, dto.Phone, dto.Password);
                return Ok(new { message = "Rejestracja przebiegła pomyślnie" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var token = await _authService.Login(dto.Email, dto.Password);
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    Email = dto.Email,
                    Message = "Logowanie przebiegło pomyślnie"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
