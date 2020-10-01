using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class FriendRequestDTO
    {
        public string Id { get; set; }

        public RequestUserDTO Stranger { get; set; }

        public bool IsActive { get; set; }
    }
}
