using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoapApi.Models;

namespace SoapApi.Data
{
    public class SoapApiContext : DbContext
    {
        public SoapApiContext (DbContextOptions<SoapApiContext> options)
            : base(options)
        {
        }

        public DbSet<Auth> Auths { get; set; }
    }
}
