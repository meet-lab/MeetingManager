using MeetingManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Models
{
    public class ThankYouModel
    {
        public Order Order { get; set; }

        public Offer Offert { get; set; }

        public UserDetail User { get; set; }
    }
}
