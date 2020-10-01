using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class RequestContainerDTO
    {
        public List<FriendRequest> MyReqs { get; set; }

        public List<FriendRequest> StrangersReqs { get; set; }
    }
}
