using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoapApi.Models;
using SoapApi.Services;

namespace SoapApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService service;
        public EventsController(EventService service)
        {
            this.service = service;
        }

        [HttpPost("newevent/{name}")]
        public async Task<ActionResult<Event>> NewEvent(string name)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            return Ok(await service.CreateEvent(name, userId));
        }
        [HttpGet("getownedevents")]
        public async Task<ActionResult<List<Event>>> GetOwnedEvents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return Ok(await service.GetOwnedEvents(userId));
        }
    }
}
