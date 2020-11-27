using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaystationGamesImporterWebJob
{
    public class Functions
    {
        public static async Task ProcessTimerAction([TimerTrigger("0 0 * * *", RunOnStartup = true)]TimerInfo timerInfo, ILogger logger)
        {
            try
            {
                var dbContext = new PlaystationWishlistContext();
                string[] regions = new[] { "en-US", "pt-BR" };
                Regex currencyRegex = new Regex(@".+?(?= ?\d)");
                Regex priceRegex = new Regex(@"\d+[,.]?\d+");

                dbContext.PlaystationGames.RemoveRange(dbContext.PlaystationGames);

                foreach (var region in regions)
                {
                    var culture = new CultureInfo(region);
                    foreach (var game in await PlaystationStoreScrapper.GetAllGames(region))
                    {
                        dbContext.PlaystationGames.Add(new PlaystationWishlist.DataAccess.Models.PlaystationGame()
                        {
                            Currency = game.FinalPrice != null ? (game.FinalPrice.Any(char.IsDigit) ? currencyRegex.Match(game.FinalPrice).Value : null) : null,
                            DiscountDescriptor = game.DiscountDescriptor,
                            FinalPrice = game.FinalPrice != null ? (game.FinalPrice.Any(char.IsDigit) ? (game.FinalPrice != null ? (decimal?)decimal.Parse(priceRegex.Match(game.FinalPrice).Value, culture) : null) : 0) : 0,
                            LastUpdataded = DateTime.UtcNow,
                            Name = game.Name,
                            OriginalPrice = game.OriginalPrice != null ? (game.OriginalPrice.Any(char.IsDigit) ? (game.OriginalPrice != null ? (decimal?)decimal.Parse(priceRegex.Match(game.OriginalPrice).Value, culture) : null) : null) : null,
                            Url = game.Url,
                            Region = game.Region
                        });
                    }
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ProcessTimerAction error.");

                throw;
            }
        }
    }
}