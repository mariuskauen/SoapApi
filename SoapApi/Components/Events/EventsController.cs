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

        [HttpGet("getattendingevents")]
        public async Task<ActionResult<List<Event>>> GetAttendingEvents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return Ok(await service.GetAttendingEvents(userId));
        }

        [HttpPost("joinevent/{eventId}")]
        public async Task<ActionResult> JoinEvent(string eventId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string status = await service.JoinEvent(userId, eventId);
            return Ok(status);
        }

        [HttpPost("leaveevent/{eventId}")]
        public async Task<ActionResult> LeaveEvent(string eventId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string status = await service.LeaveEvent(userId, eventId);
            return Ok(status);
        }
    }
}
