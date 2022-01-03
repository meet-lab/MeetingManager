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

        public DbSet<MeetingManager.Models.Offer> Offer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add the shadow property to the model
            modelBuilder.Entity<MeetingManager.Models.Offer>()
                .Property<int>("UserForeignKey");

            // Use the shadow property as a foreign key
            modelBuilder.Entity<MeetingManager.Models.Offer>()
                .HasOne(p => p.User)
                .WithMany(b => b.Offers)
                .HasForeignKey("UserForeignKey");
        }
    }
}
