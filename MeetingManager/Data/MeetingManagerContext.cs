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
        public MeetingManagerContext(DbContextOptions<MeetingManagerContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<UserDetail> UserDetail { get; set; }

        public DbSet<Offer> Offer { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<CartLineItem> CartLineItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,4)");
        }
    }
}
