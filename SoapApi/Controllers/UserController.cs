using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoapApi.Models;
using SoapApi.Services;

namespace SoapApi.Controllers
{
    [Route("api/user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _user;

        public UserController(UserService userS)
        {
            _user = userS;
        }
        [HttpGet("getownuserfull")]
        public async Task<ActionResult<FullUserDTO>> GetOwnUserFull()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string query = "Users:_id:" + userId;
            return Ok(await _user.GetUser(query));
        }
    }
}