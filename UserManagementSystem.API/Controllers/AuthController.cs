using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.API.Dtos;
using UserManagementSystem.API.Interfaces;
using UserManagementSystem.API.Mappings;
using UserManagementSystem.API.Models;
using UserManagementSystem.API.Repositories;
using UserManagementSystem.API.Services;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthController(IAuthRepository authRepository, IUserRepository userRepository, JwtService jwtService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _authRepository.AuthenticateAsync(loginDto.Username, loginDto.Password);
                if(user == null) return Unauthorized(new { message = "Invalid username or password" });

                var token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            } catch(Exception ex)
            {
                return StatusCode(500, new { message  = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RequestUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 6)
                {
                    return BadRequest(new { message = "Password is required and must be at least 6 characters." });
                }

                var user = userDto.ToModel(_jwtService.HashPassword(userDto.Password));
                var createdUser = await _userRepository.CreateAsync(user);

                return Ok(createdUser.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
