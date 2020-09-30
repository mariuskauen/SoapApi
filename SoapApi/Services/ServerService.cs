using SoapApi.Data.Repositories;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class ServerService
    {
        private readonly CommandRepository command;
        private readonly QueryRepository queryRepo;
        public ServerService(CommandRepository command, QueryRepository query)
        {
            this.command = command;
            queryRepo = query;
        }
        public async Task CreateServer(string serverName, string userId)
        {
            Server server = new Server()
            {
                Id = Guid.NewGuid().ToString(),
                OwnerId = userId,
                Name = serverName,
                UserIds = new List<string>() { userId },
                ChannelIds = new List<string>() { }
            };
            server.ChannelIds.Add(await CreateFirstChannel("General", server.Id));
            string query = "Servers";
            await command.Post(server, query);
            return;
        }
        public async Task<string> CreateFirstChannel(string name, string serverid)
        {
            string query = "Channels";
            Channel chan = new Channel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                ServerId = serverid
            };
            await command.Post(chan, query);
            return chan.Id;
        }
    }
}
