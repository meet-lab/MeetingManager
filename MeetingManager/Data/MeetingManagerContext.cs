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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Add the shadow property to the model
            modelBuilder.Entity<Offer>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,4)");
        }

        public DbSet<MeetingManager.Models.Order> Order { get; set; }

        public DbSet<MeetingManager.Models.Cart> Cart { get; set; }

        public DbSet<MeetingManager.Models.CartLineItem> CartLineItem { get; set; }
    }
}
