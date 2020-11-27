using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlaystationWishlist.DataAccess.Models
{
    public class PlaystationGame
    {
#nullable enable
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Currency { get; set; }
        [Column(TypeName = "decimal (18,2)")]
        public decimal? FinalPrice { get; set; }
        [Column(TypeName = "decimal (18,2)")]
        public decimal? OriginalPrice { get; set; }
        public string? DiscountDescriptor { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime LastUpdataded { get; set; }
        [Required]
        [StringLength(5)]
        public string Region { get; set; }
#nullable disable
    }
}
