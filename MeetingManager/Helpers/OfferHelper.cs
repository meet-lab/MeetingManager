using MeetingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Helpers
{
    public class OfferHelper
    {
        public bool FilterByOfferStatus(Offer offer, string offerStatus)
        {
            if (offerStatus == null)
            {
                return true;
            }
            return offer.Status == offerStatus;
        }

        public bool FilterByOfferName(Offer offer, string offerTitle)
        {
            if (offerTitle == null)
            {
                return true;
            }
            return offer.Title == offerTitle;
        }
    }
}
