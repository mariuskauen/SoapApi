using SoapApi.Data.Repositories;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class ChannelService
    {
        private readonly CommandRepository command;
        private readonly ServerService server;
        public ChannelService(CommandRepository command, ServerService server)
        {
            this.command = command;
            this.server = server;
        }

        public async Task CreateChannel(string name, string serverid)
        {
            string query = "Channels";
            Channel chan = new Channel()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                ServerId = serverid
            };
            await command.Post(chan, query);
            query = "Servers:_id:" + serverid + ":insertinto:ChannelIds:" + chan.Id;
            await command.Put(chan, query);
            return;
        }
        //For å forandre en variabel:
        //query = "Servers:_id:" + serverid + ":set:ownerId:" + newOwnerId;

    }
}
