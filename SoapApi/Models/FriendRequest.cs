using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class FriendRequest
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public bool IsActive { get; set; }
    }
}
