using System;
using System.Collections.Generic;
using System.Text;

namespace PlaystationWishlistAPI.Models
{

    public class InDeleteWishlistItem
    {
        public int UserId { get; set; }
        public string GameUrl { get; set; }
    }
}
