using System;
using System.Collections.Generic;
using System.Text;

namespace PlaystationWishlist.Core.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string OIdProvider { get; set; }
        public string OId { get; set; }
    }
}
