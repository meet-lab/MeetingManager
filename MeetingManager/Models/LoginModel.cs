using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User name or login")]
        public string EmailAddressOrUserName { get; set; }
        [Required]
        [Display(Name = "User password")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
