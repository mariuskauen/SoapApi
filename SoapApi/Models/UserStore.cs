using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Models
{
    public class UserStore
    {
        public string Id { get; set; }
        public List<string> UserIds { get; set; }
    }
}
