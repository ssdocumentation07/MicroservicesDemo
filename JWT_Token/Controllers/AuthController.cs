using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _settings;

        public AuthController(IConfiguration settings)
        {
            _settings = settings;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {

            try
            {
                var response = await LoginAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }
        private async Task<string> LoginAsync()
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(ClaimTypes.Name, "sarthak"),
                new Claim(ClaimTypes.Email, "test@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings["Jwt:Issuer"],
                audience: _settings["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        

    }
}
}
