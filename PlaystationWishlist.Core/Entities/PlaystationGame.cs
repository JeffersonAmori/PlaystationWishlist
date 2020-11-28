using System;

namespace PlaystationWishlist.Core.Entities
{
    public class PlaystationGame
    {
        private string finalPrice;
        private string originalPrice;
        private string currency;

        public PlaystationGame(string name, string finalPrice, string originalPrice, string discountDescriptor, string url, string region, string currency)
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

            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException($"'{nameof(currency)}' cannot be null or empty", nameof(currency));
            }

            Name = name;
            this.finalPrice = finalPrice;
            this.originalPrice = originalPrice;
            DiscountDescriptor = discountDescriptor;
            Url = url;
            Region = region;
            this.currency = currency;
        }

        public string Name { get; }
        public string FinalPrice { get => currency + finalPrice; }
        public string OriginalPrice { get => originalPrice == null ? null : currency + originalPrice; }
        public string DiscountDescriptor { get; }
        public string Url { get; }
        public string Region { get; }
    }
}
