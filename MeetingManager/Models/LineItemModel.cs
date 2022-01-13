using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingManager.Models
{
    public class LineItemModel
    {
        public Offer Offer { get; set; }

        public int OfferId { get; set; }

        public int CartId { get; set; }

        [Required]
        [Display(Name = "When you want to book in?")]
        public DateTime From { get; set; }

        [Required]
        [Display(Name = "When you want to book out?")]
        public DateTime To { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
