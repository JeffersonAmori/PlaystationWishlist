using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlaystationWishlist.DataAccess.Models
{
    public class WishlistItem
    {
        public long Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string GameUrl { get; set; }
    }
}
