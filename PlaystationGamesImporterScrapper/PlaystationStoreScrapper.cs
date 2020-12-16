using HtmlAgilityPack;
using PlaystationWishlist.Core.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PlaystationGamesLoadScrapper
{
    public static class PlaystationStoreScrapper
    {
        private static readonly string basePsnAddress = "https://store.playstation.com";

        private static readonly Dictionary<string, string> platforms = new Dictionary<string, string>()
            {
                {"PS5", "d71e8e6d-0940-4e03-bd02-404fc7d31a31"},
                {"PS4", "85448d87-aa7b-4318-9997-7d25f4d275a4"}
            };


        public static async Task<IEnumerable<PlaystationGame>> GetAllGamesByRegion(params string[] regions)
        {
            var count = 1;
            var pageNumber = 1;
            var isLastPage = false;
            var htmlWeb = new HtmlWeb();
            IList<PlaystationGame> gamesList = new List<PlaystationGame>();
            ConcurrentBag<Task> listOfTasks = new ConcurrentBag<Task>();
            Regex currencyRegex = new Regex(@".+?(?= ?\d)");

            foreach (var region in regions)
            {
                foreach (var platform in platforms)
                {
                    pageNumber = 1;
                    isLastPage = true;
                    do
                    {
                        var doc = htmlWeb.Load($"{basePsnAddress}/{region}/category/{platform.Value}/{pageNumber++}");
                        if (doc.DocumentNode.Descendants().Any(d => d.Attributes["data-qa"]?.Value == "ems-sdk-grid-paginator-next-page-btn" && (bool)d.Attributes["class"]?.Value.Contains("psw-is-disabled")))
                        {
                            isLastPage = true;
                        }
                        var gameLinks = doc.DocumentNode.Descendants().Where(d => d.Attributes["class"]?.Value == "ems-sdk-product-tile-link");
                        foreach (var link in gameLinks)
                        {
                            PlaystationGame playstationGame = null;

                            var gameUrl = basePsnAddress + link.Attributes["href"].Value;
                            var _ = Task.Run(() =>
                            {
                                int retryCount = 0;
                                PROCESS_GAME:
                                try
                                {
                                    return htmlWeb.Load(gameUrl);
                                }
                                catch (System.Net.WebException)
                                {
                                    if (retryCount > 20)
                                        throw;

                                    retryCount++;
                                    Thread.Sleep(TimeSpan.FromSeconds(2));
                                    Console.WriteLine("Retrying for the " + retryCount + " time");
                                    goto PROCESS_GAME;
                                }
                            })
                                .ContinueWith(async gamePageTask =>
                                {
                                    var gamePage = await gamePageTask;
                                    var gameName = gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "mfe-game-title#name")?.InnerText;
                                    var gameFinalPrice = gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#finalPrice")?.InnerText;
                                    var gameOriginalPrice = gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#originalPrice")?.InnerText;
                                    var gameDiscountDescriptor = gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#discountDescriptor")?.InnerText;
                                    var gameCurrency = currencyRegex.Match(gameFinalPrice ?? string.Empty).Value;
                                    var gameImageUrl = gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "gameBackgroundImage#heroImage#preview")?.Attributes["src"]?.Value?.Split("?")[0] ??
                                                       gamePage.DocumentNode.Descendants().FirstOrDefault(d => d.Attributes["data-qa"]?.Value == "gameBackgroundImage#tileImage#preview")?.Attributes["src"]?.Value?.Split("?")[0];
                                    var gamePlatform = platform.Key;

                                    gamesList.Add(new PlaystationGame(gameName, gameFinalPrice, gameOriginalPrice, gameDiscountDescriptor, gameUrl, region, gameCurrency, gameImageUrl, gamePlatform));

                                    Console.WriteLine($"{count++} - {gamePlatform} - {gameName} - {gameFinalPrice} - {gameOriginalPrice} - {gameDiscountDescriptor}");
                                });
                            listOfTasks.Add(_);
                        }
                    }
                    while (!isLastPage);
                }
            }

            await Task.WhenAll(listOfTasks);
            return gamesList;
        }
    }
}
