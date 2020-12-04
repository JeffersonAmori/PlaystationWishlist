using System;

namespace PlaystationWishlist.Core.Entities
{
    public class PlaystationGame
    {
        private readonly string _finalPrice;
        private readonly string _originalPrice;
        private readonly string _currency;
        private readonly string _gameImageUrl;
        private double? _discountPercentage;
        private bool _isOnUserWishlist;

        public PlaystationGame(string name, string finalPrice, string originalPrice, string discountDescriptor, string url, string region, string currency, string gameImageUrl, string gamePlatform, double? discountPercentage = null, bool isOnUserWishlist = false)
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

            //if (string.IsNullOrEmpty(currency))
            //{
            //    throw new ArgumentException($"'{nameof(currency)}' cannot be null or empty", nameof(currency));
            //}

            if (string.IsNullOrEmpty(gameImageUrl))
            {
                throw new ArgumentException($"'{nameof(gameImageUrl)}' cannot be null or empty", nameof(gameImageUrl));
            }

            if (string.IsNullOrEmpty(gamePlatform))
            {
                throw new ArgumentException($"'{nameof(gamePlatform)}' cannot be null or empty", nameof(gamePlatform));
            }

            Name = name;
            _finalPrice = finalPrice;
            _originalPrice = originalPrice;
            DiscountDescriptor = discountDescriptor;
            Url = url;
            Region = region;
            _currency = currency;
            _gameImageUrl = gameImageUrl;
            _discountPercentage = discountPercentage;
            _isOnUserWishlist = isOnUserWishlist;
            GamePlatform = gamePlatform;
        }

        public string Name { get; }
        public string FinalPrice => _finalPrice;
        public string OriginalPrice => _originalPrice;
        public string DiscountDescriptor { get; }
        public string Url { get; }
        public string Region { get; }
        public string GamePlatform { get; }
        public string Currency => _currency;

        public bool IsOnUserWishlist
        {
            get => _isOnUserWishlist;
            set => _isOnUserWishlist = value;
        }
        public string GameImageUrl => _gameImageUrl;

        public string FinalPriceWithCurrency => _currency + _finalPrice;
        public string OriginalPriceWithCurrency => _currency + _originalPrice ?? string.Empty;

        public double? DiscountPercentage
        {
            get => _discountPercentage;
            set => _discountPercentage = value;
        }
    }
}
