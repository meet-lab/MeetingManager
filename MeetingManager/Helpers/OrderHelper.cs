using MeetingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Helpers
{
    public class OrderHelper
    {
        public bool FilterOrderStatus(Order order, string status)
        {
            if (status == "Saved")
            {
                return order.To < DateTime.Now;
            }

            if (order.Status == status)
            {
                return true;
            }

            return false;
        } 
    }
}
