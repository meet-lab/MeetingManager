using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingManager.Models
{
    public class UserDetail
    {
        public int UserDetailId { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string SecondName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        public string City { get; set; }

        [DataType(DataType.Text)]
        public string Region { get; set; }

        [DataType(DataType.Text)]
        public string Country { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}
