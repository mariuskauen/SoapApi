using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoapApi.Models;
using SoapApi.Services;

namespace SoapApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService auth;
        private readonly IConfiguration config;

        public AuthController(AuthService auth, IConfiguration config)
        {
            this.auth = auth;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register reg)
        {
            if (await auth.UsernameExists(reg.Username.ToLower()))
                return BadRequest("Username already exists!");

            User userToCreate = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = reg.Username,
                Status = Status.Online,
                DateJoined = DateTime.UtcNow,
                LastOnline = DateTime.UtcNow,
                ProfilePicture = "blabla"
            };

            while (await auth.IdExists(userToCreate.Id))
            {
                userToCreate.Id = Guid.NewGuid().ToString();
            }

            if (await auth.Register(userToCreate, reg.Password))
                return Ok();

            return StatusCode(400);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login vm)
        {
            User userFromRepo = await auth.Login(vm.Username.ToLower(), vm.Password);
            if (userFromRepo == null)
            {
                return BadRequest();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}