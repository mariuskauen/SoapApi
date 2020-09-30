using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoapApi.Services;

namespace SoapApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly ServerService serverService;
        public ServerController(ServerService serverService)
        {
            this.serverService = serverService;
        }

        [HttpPost("createserver/{name}")]
        public async Task<ActionResult> CreateServer(string name)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await serverService.CreateServer(name, userId);

            return Ok();
        }
    }
}
