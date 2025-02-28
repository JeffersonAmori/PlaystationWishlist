﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite.Models
{
    public class GamesListViewModel
    {
        public  IEnumerable<GamesViewModel> GamesViewModels;
        public bool ShowSearchBar;
    }

    public class GamesViewModel
    {
        public string Name { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string DiscountDescriptor { get; set; }
        public string Url { get; set; }
        public string Region { get; set; }
        public string Currency { get; set; }
        public bool IsOnUserWishlist { get; set; }
        public string GameImageUrl { get; set; }
        public string GamePlatform { get; set; }
        
    }
}
