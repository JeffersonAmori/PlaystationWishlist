using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlaystationWishlist.DataAccess.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string OIdProvider { get; set; }
        public string OId { get; set; }
    }
}
