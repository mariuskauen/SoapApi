﻿using System;
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
    [ApiController]
    [Authorize]
    public class RelationsController : ControllerBase
    {
        private readonly RelationsService service;
        public RelationsController(RelationsService service)
        {
            this.service = service;
        }

        [HttpPost("sendrequest/{receiverId}")]
        public async Task<IActionResult> SendFriendRequest(string receiverId)
        {
            List<FriendRequest> requests = new List<FriendRequest>();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await service.CheckIfUserExist(userId))
                return BadRequest("Who are you? O_o o_O");
            if (await service.CheckIfUserExist(receiverId))
                return BadRequest("No such user, dude!");

            requests = await service.GetRequestsForSend(userId);

            foreach(FriendRequest fr in requests)
            {
                if(fr.ReceiverId == receiverId)
                {
                    if (fr.IsActive)
                        return BadRequest("There already is an active requests.");
                    fr.IsActive = true;
                    return Ok();
                }
                if(fr.SenderId == receiverId)
                {
                    if(fr.IsActive)
                    {
                        //Add as friend
                        return Ok();
                    }    
                }
            }

            FriendRequest req = new FriendRequest()
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = userId,
                ReceiverId = receiverId,
                IsActive = true
            };

            await service.SendRequest(req);

            return Ok();
        }

        [HttpGet("getmyrequests")]
        public async Task<ActionResult<List<User>>> GetMyRequests()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return await service.GetMyRequests(userId);
        }
        [HttpGet("getinboundrequests")]
        public async Task<ActionResult<List<User>>> GetInboundRequests()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return await service.GetInboundRequests(userId);
        }

        [HttpPost("acceptrequest/{friendId}")]
        public async Task<ActionResult> AcceptRequest(string friendId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await service.AcceptRequest(userId, friendId);

            return Ok();
        }

        [HttpGet("getmyfriends")]
        public async Task<ActionResult<List<User>>> GetMyFriends()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await service.GetMyFriends(userId);
        }
    }
}
