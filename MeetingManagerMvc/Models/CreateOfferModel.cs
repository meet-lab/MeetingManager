using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Models
{
    public class CreateOfferModel
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [Required]
        [Range(0,999999)]
        public decimal Price { get; set; }
        public int UserId { get; set; }
    }
}
