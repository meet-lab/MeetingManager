using System;
using System.ComponentModel.DataAnnotations;

namespace MeetingManagerMvc.Models
{
    public class UserRegistryModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Repeat Password")]
        public string RepeatPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
