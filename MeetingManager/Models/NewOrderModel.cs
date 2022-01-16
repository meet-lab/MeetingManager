using MeetingManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class NewOrderModel
    {
        public Offer Offer { get; set; }

        public CartLineItem CartLineItem { get; set; }

        public string Comment { get; set; }

        public string UserId { get; set; }
    }
}
