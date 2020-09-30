using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class Channel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ServerId { get; set; }
    }
}
