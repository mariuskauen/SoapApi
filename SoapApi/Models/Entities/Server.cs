using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class Server
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public List<string> UserIds { get; set; }

        public List<string> ChannelIds { get; set; }
    }
}
