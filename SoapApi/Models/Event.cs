using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class Event
    {
        public string Id { get; set; }

        public string Navn { get; set; }

        public string OwnerId { get; set; }
    }
}
