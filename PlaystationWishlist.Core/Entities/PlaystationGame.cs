using System;

namespace PlaystationWishlist.Core.Entities
{
    public class PlaystationGame
    {
        public PlaystationGame(string name, string finalPrice, string originalPrice, string discountDescriptor, string url, string region)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException($"'{nameof(url)}' cannot be null or empty", nameof(url));
            }

            if (string.IsNullOrEmpty(region))
            {
                throw new ArgumentException($"'{nameof(region)}' cannot be null or empty", nameof(region));
            }

            Name = name;
            FinalPrice = finalPrice;
            OriginalPrice = originalPrice;
            DiscountDescriptor = discountDescriptor;
            Url = url;
            Region = region;
        }

        public string Name { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string DiscountDescriptor { get; set; }
        public string Url { get; set; }
        public string Region { get; set; }
    }
}
