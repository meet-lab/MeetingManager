using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class LoginModel
    {
        public string EmailAddressOrUserName { get; set; }
        public string Password { get; set; }
    }
}
