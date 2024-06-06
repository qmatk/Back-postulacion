using crud_books_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crud_books_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration configuration;
        public AuthController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Auth([FromBody] Authorization authorization)
        {
            if (authorization == null) return BadRequest(new { message = "Ingrese credenciales" });

            if (authorization.Username != null) return BadRequest(new { message = "Debe ingresar usuario" });
            if (authorization.Password != null) return BadRequest(new { message = "Debe ingresar password" });

            if (authorization.Username != "auth_postulacion" &&  authorization.Password != "pass_postulacion") return BadRequest(new { message = "Credenciales inválidas" });

            string JWTToken = GenerateToken(authorization);
            

            return Ok(new { token = JWTToken });
        }

        private string GenerateToken(Authorization authorization)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authorization.Username)
            };

            var secretKey = configuration.GetSection("JWT:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securitytoken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(securitytoken);

            return token;
        }
    }
}
