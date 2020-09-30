using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class FullUserDTO
    {
        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public Status Status { get; set; }

        //public List<Conversation> Conversations { get; set; }

        //public List<Server> Servers { get; set; }

        //public List<FriendShip> FriendShips { get; set; }

        //public List<FriendRequest> FriendRequests { get; set; }

        public string Settings { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime LastOnline { get; set; }
    }
}
