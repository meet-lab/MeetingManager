using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class Offer
    {

        public enum StatusType
        {
            Archived,
            Draft,
            Published
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusType Status { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
