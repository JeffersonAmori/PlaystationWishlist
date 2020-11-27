using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using PlaystationWishlist.Core.Interfaces;
using System;

namespace PlaystationWishlist.KeyVault
{
    public class KeyVaultService : IKeyVaultService
    {
        private SecretClient client;

        public KeyVaultService(SecretClientOptions options = null)
        {
            SecretClientOptions defaultOptions = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            client = new SecretClient(new Uri("https://PlayStationWishlistKV.vault.azure.net/"), new DefaultAzureCredential(), options ?? defaultOptions);
        }

        public string GetSecret(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            return client.GetSecret(name).Value.Value ?? throw new NullReferenceException("Null referece exception while retrieving secret");
        }
    }
}
