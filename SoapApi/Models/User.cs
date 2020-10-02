using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime LastOnline { get; set; }
    }
}
