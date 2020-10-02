using SoapApi.Data;
using SoapApi.Data.Repositories;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class EventService
    {
        private readonly CommandRepository command;
        private readonly SoapApiContext context;
        private readonly QueryRepository query;
        public EventService(CommandRepository command, SoapApiContext context, QueryRepository query)
        {
            this.command = command;
            this.context = context;
            this.query = query;
        }
        public async Task CreateEvent(string name, string userId)
        {

            Event newEvent = new Event()
            {
                Id = Guid.NewGuid().ToString(),
                Navn = name,
                OwnerId = userId
            };
            UserStore userStore = new UserStore()
            {
                Id = newEvent.Id,
                UserIds = new List<string>() { userId }
            };

            string queryString = "UserStore";
            //await command.Post(userStore, queryString);
            //await context.Events.AddAsync(newEvent);
            //await context.SaveChangesAsync();

            return;
        }

        public async Task<UserStore> GetEventUsers(string eventId)
        {
            string queryString = "UserStore:_id:"+ eventId;
            return await query.GetSingle(new UserStore(), new UserStore(), queryString);
        }
    }
}
