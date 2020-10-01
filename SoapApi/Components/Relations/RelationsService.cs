using Microsoft.EntityFrameworkCore;
using SoapApi.Data;
using SoapApi.Data.Repositories;
using SoapApi.Helpers;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class RelationsService
    {
        private readonly SoapApiContext context;
        private readonly MapConfig mapConfig;

        public RelationsService(SoapApiContext context, MapConfig mapConfig)
        {
            this.context = context;
            this.mapConfig = mapConfig;
        }
        public async Task<bool> CheckIfUserExist(string userId)
        {
            return (await context.Users.FirstOrDefaultAsync(x => x.Id == userId) == null);
        }

        public async Task<List<FriendRequest>> GetRequestsForSend(string userId)
        {
            List<FriendRequest> reqs = await context.FriendRequest.Where(x => x.SenderId == userId).ToListAsync();
            reqs.AddRange(await context.FriendRequest.Where(x => x.ReceiverId == userId).ToListAsync());

            return reqs;
        }
        public async Task<RequestContainerDTO> GetRequests(string userId)
        {
            RequestContainerDTO dto = new RequestContainerDTO();
            dto.MyReqs = await context.FriendRequest.Where(x => x.SenderId == userId).ToListAsync();
            dto.StrangersReqs = await context.FriendRequest.Where(x => x.ReceiverId == userId).ToListAsync();

            return dto;
        }

        public async Task SendRequest(FriendRequest req)
        {
            await context.FriendRequest.AddAsync(req);
            await context.SaveChangesAsync();
            return;
        }

        public async Task<string> AcceptRequest(string userId, string requestId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return "Who are you? O_o o_O";

            FriendRequest req = await context.FriendRequest.FirstOrDefaultAsync(x => x.Id == requestId);
            if (req == null)
                return "Could not find request";

            if (!req.IsActive)
                return "Can't accept, inactive friend request";
            if (req.SenderId == userId)
                return "You can't accept your own request!";
            if (req.ReceiverId != userId)
                return "Who's request are you trying to accept here?";

            Friendship fs = new Friendship()
            {
                Id = req.SenderId + ":" + req.ReceiverId,
                IsActive = true
            };
            req.IsActive = false;
            context.Friendships.Add(fs);
            await context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<List<Friendship>> GetMyFriends(string userId)
        {
            return await context.Friendships.Where(s => s.Id.Contains(userId)).ToListAsync();
        }
    }
}
