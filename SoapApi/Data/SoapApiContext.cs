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

        public DbSet<FriendRequest> FriendRequest { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Event> Events { get; set; }
    }
}
