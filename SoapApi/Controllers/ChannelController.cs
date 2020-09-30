using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoapApi.Services;

namespace SoapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelService chan;
        public ChannelController(ChannelService chan)
        {
            this.chan = chan;
        }

        [HttpPost("createnew/{serverid}/{name}")]
        public async Task<ActionResult> CreateNew(string serverid, string name)
        {
            await chan.CreateChannel(name, serverid);
            return Ok();
        }

    }
}
