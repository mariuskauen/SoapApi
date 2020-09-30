using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SoapApi.Components.Mediator;
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
            reg.Username = reg.Username.ToLower();
            if (await auth.UserExists(reg.Username))
                return BadRequest("Username already exists!");
            var userToCreate = new Auth
            {
                Id = Guid.NewGuid().ToString(),
                Username = reg.Username
            };

            while (await auth.IdExists(userToCreate.Id))
            {
                userToCreate.Id = Guid.NewGuid().ToString();
            }

            var createdUser = await auth.Register(userToCreate, reg.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login vm)
        {
            var userFromRepo = await auth.Login(vm.Username.ToLower(), vm.Password);
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