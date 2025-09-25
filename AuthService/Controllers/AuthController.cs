using AuthService.DTOs;
using AuthService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtService jwtService,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            // Create User
            var exists = await _userManager.FindByNameAsync(user.UserName);
            
            if(exists != null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                });
            }
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

            // Generate Jwt Token
            var token = await _jwtService.GenerateJwtToken(user);
            var jwtSettings = _config.GetSection("JwtSettings");
            var expirationMinutes = Convert.ToDouble(jwtSettings["ExpirationInMinutes"]);

            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutes),
                UserId = user.Id,
                Email = user.Email,
                Success = true,
                Message = "Registration successful"
            });

        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            
            // Generate Token
            var token = await _jwtService.GenerateJwtToken(user);
            var jwtSettings = _config.GetSection("JwtSettings");
            var expirationMinutes = Convert.ToDouble(jwtSettings["ExpirationInMinutes"]);

            return Ok(new AuthResponseDto
            {
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddMinutes(expirationMinutes),
                UserId = user.Id,
                Email = user.Email,
                Success = true,
                Message = "Login successful"
            }
            );


        }
    }
}
