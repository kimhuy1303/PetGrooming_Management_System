using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Services;
namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<AuthController> _logger;
            private readonly JWTService _jwtService;

            public AuthController(IUserRepository userRepository, ILogger<AuthController> logger, JWTService jWTService)
            {
                _userRepository = userRepository;
                _logger = logger;
                _jwtService = jWTService;
            }

            [HttpPost("Login")]
            public async Task<ActionResult<User>> Login([FromBody] DTOs.Requests.LoginRequest loginDTO)
            {
                try
                {
                    var _user = _userRepository.GetUserByUsername(loginDTO.Username!);
                    
                    if (_user == null || !_userRepository.verifyPassword(_user.Result, loginDTO.Password!))
                    {
                    return BadRequest("Username or password is invalid");
                    }
                // Generate token jwt
                var token = _jwtService.generateJwtToken(_user.Result.Username!, _user.Result.Role, _user.Result.PhoneNumber!);

                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, "Error login");
                    return BadRequest("Login errored");
                }
            }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest registerDTO)
        {
            var check = _userRepository.GetUserByUsername(registerDTO.Username!);
            if (check == null) { return BadRequest("Username already exists!"); }
            if (!registerDTO.Password!.Equals(registerDTO.ConfirmPassword)) return BadRequest("Passwords do not match!");
            var newUser = new UserRequest
            {
                UserName = registerDTO.Username!,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password!),
                PhoneNumber = registerDTO.PhoneNumber
            };
            var res = await _userRepository.createUser(newUser);
            if (!res) return BadRequest("Create failed"); 
            else return Ok("Create successful");  

        }
    }   
}
