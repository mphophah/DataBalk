using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserTaskApi.Services;

namespace UserTaskApi.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        private readonly JwtService _jwtService;

        public UserController(IConfiguration configuration, IUserRepository userRepository, JwtService jwtService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [AllowAnonymous]// Endpoint to allow unauthenticated users to register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Check if the provided username or email already exists
            if (await _userRepository.UsernameOrEmailExistsAsync(registerDto.Username, registerDto.Email))
            {
                return BadRequest("Username or email is already in use.");
            }

            await _userRepository.CreateUserAsync(registerDto);

            return Ok(new { Message = "User registered successfully" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !_userRepository.VerifyPassword(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate a JWT token for the authenticated user
            var token = _jwtService.GenerateJwtToken(user.Username);

            return Ok(new { Token = token });
        }

        [HttpDelete("delete/{username}")]
        [Authorize]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username); // Find the user by username and delete their account

            if (user == null)
            {
                return NotFound("User not found");
            }

            _userRepository.DeleteUserAsync(user);

            return Ok(new { Message = "User deleted successfully" });
        }

        [HttpPut("update/{username}")]
        [Authorize]
        public async Task<IActionResult> Update(string username, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username); // Find the user by username and update their account information

            if (user == null)
            {
                return NotFound("User not found");
            }

            _userRepository.UpdateUserAsync(user);

            return Ok(new { Message = "User updated successfully" });
        }

    }
}