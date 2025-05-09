using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using chatappapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace chatappapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Simulated in-memory user list
        private static List<User> Users = new List<User>();

        [HttpPost("register")]
        public IActionResult Register(UserDTO userDTO)
        {
            if (Users.Any(u => u.Username == userDTO.Username))
            {
                return BadRequest("Username already exists");
            }

            var user = new User
            {
                Id = Users.Count + 1,
                Username = userDTO.Username,
                Password = userDTO.Password
            };

            Users.Add(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserDTO userDTO)
        {
            var user = Users.FirstOrDefault(u => u.Username == userDTO.Username && u.Password == userDTO.Password);
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
