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
        private readonly AuthService _auth;
        private readonly IConfiguration config;
        private readonly UserService _user;

        public AuthController(AuthService auth, IConfiguration config, UserService user)
        {
            _auth = auth;
            this.config = config;
            _user = user;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register reg)
        {
            if (await _auth.UsernameExists(reg.Username.ToLower()))
                return BadRequest("Username already exists!");

            Auth auth = new Auth()
            {
                Id = Guid.NewGuid().ToString(),
                Username = reg.Username
            };

            while (await _auth.IdExists(auth.Id))
            {
                auth.Id = Guid.NewGuid().ToString();
            }

            if (await _auth.Register(auth, reg.Password))
            {
                await _user.InitializeUser(auth.Id, auth.Username);
                return Ok();
            }
            return StatusCode(400);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login vm)
        {
            Auth authFromRepo = await _auth.Login(vm.Username.ToLower(), vm.Password);
            if (authFromRepo == null)
            {
                return BadRequest();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, authFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, authFromRepo.Username)
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