using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.API.Dtos;
using UserManagementSystem.API.Interfaces;
using UserManagementSystem.API.Mappings;
using UserManagementSystem.API.Services;

namespace UserManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserController(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(u => u.ToDto());
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 6)
                {
                    return BadRequest(new { message = "Password must be at least 6 characters." });
                }

                var hashedPassword = _jwtService.HashPassword(userDto.Password);
                var user = userDto.ToModel(hashedPassword);

                var createdUser = await _userRepository.CreateAsync(user);
                return Ok(createdUser.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RequestUserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(userDto.Password) || userDto.Password.Length < 6)
                {
                    return BadRequest(new { message = "Password must be at least 6 characters." });
                }

                var hashedPassword = _jwtService.HashPassword(userDto.Password);
                var updatedUserModel = userDto.ToModel(hashedPassword);

                var updatedUser = await _userRepository.UpdateAsync(id, updatedUserModel);
                return updatedUser == null ? NotFound() : Ok(updatedUser.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _userRepository.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
