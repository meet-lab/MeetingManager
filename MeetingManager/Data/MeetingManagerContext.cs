using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MeetingManager.Models;

namespace MeetingManager.Data
{
    public class MeetingManagerContext : DbContext
    {
        public MeetingManagerContext (DbContextOptions<MeetingManagerContext> options)
            : base(options)
        {
        }

        public DbSet<MeetingManager.Models.User> User { get; set; }
    }
}
