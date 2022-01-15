using MeetingManager.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeetingManagerMvc.Models
{
    public class FakeDataContext : DbContext
    {
        public FakeDataContext(DbContextOptions<FakeDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserDetail> UserDetail { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<CartLineItem> CartLineItem { get; set; }
    }
}
