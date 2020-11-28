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

        public async static Task<IEnumerable<PlaystationGame>> GetAllGames(params string[] regions)
        {
            int count = 1;
            int pageNumber = 1;
            bool isLastPage = false;
            var htmlWeb = new HtmlWeb();
            IList<PlaystationGame> gamesList = new List<PlaystationGame>();
            ConcurrentBag<Task> listOfTasks = new ConcurrentBag<Task>();
            Regex currencyRegex = new Regex(@".+?(?= ?\d)");

            foreach (var region in regions)
            {
                do
                {
                    var doc = htmlWeb.Load($"{basePsnAddress}/{region}/category/85448d87-aa7b-4318-9997-7d25f4d275a4/{pageNumber++}");
                    if (doc.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "ems-sdk-grid-paginator-next-page-btn" && (bool)d.Attributes["class"]?.Value.Contains("psw-is-disabled")).Any())
                    {
                        isLastPage = true;
                    }
                    var gameLinks = doc.DocumentNode.Descendants().Where(d => d.Attributes["class"]?.Value == "ems-sdk-product-tile-link");
                    foreach (var link in gameLinks)
                    {
                        PlaystationGame playstationGame = null;

                        string gameUrl = basePsnAddress + link.Attributes["href"].Value;
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
                                string gameName = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfe-game-title#name").FirstOrDefault()?.InnerText;
                                string gameFinalPrice = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#finalPrice").FirstOrDefault()?.InnerText;
                                string gameOriginalPrice = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#originalPrice").FirstOrDefault()?.InnerText;
                                string gameDescountDescriptor = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#discountDescriptor").FirstOrDefault()?.InnerText;
                                string gameCurrency = currencyRegex.Match(gameFinalPrice).Value;
                                Console.WriteLine($"{count++} - {gameName} - {gameFinalPrice} - {gameOriginalPrice} - {gameDescountDescriptor}");

                                gamesList.Add(new PlaystationGame(gameName, gameFinalPrice, gameOriginalPrice, gameDescountDescriptor, gameUrl, region, gameCurrency));
                            });
                        listOfTasks.Add(_);
                    }
                }
                while (!isLastPage);
            }

            await Task.WhenAll(listOfTasks);
            return gamesList;
        }
    }
}
