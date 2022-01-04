﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class User
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<Offer> Offers { get; set; }
    }
}
