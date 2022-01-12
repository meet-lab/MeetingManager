using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class CartLineItem
    {
        public int Id { get; set; }

        [ForeignKey("CartId")]
        public int CartId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public virtual Offer Offer { get; set; }
    }
}
