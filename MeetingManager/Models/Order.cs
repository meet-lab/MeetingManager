using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public int OfferId { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set;}

        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Comment { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EditDate { get; set; }
    }
}
