using MeetingManager.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeetingManagerMvc.Models
{
    public class FakeDataContext: DbContext
    {
        public FakeDataContext(DbContextOptions<FakeDataContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
