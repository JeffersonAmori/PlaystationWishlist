using HtmlAgilityPack;
using System;
using System.Linq;

namespace PlaystationGamesLoadScrapper
{
    public static class PlaystationStoreScrapper
    {
        private static readonly string basePsnAddress = "https://store.playstation.com";
        private static readonly string[] zones = new[] { "pt-br" };

        public static void GetAllGames()
        {
            int count = 1;
            int pageNumber = 1;
            bool isLastPage = false;
            var htmlWeb = new HtmlWeb();

            foreach (var zone in zones)
            {
                do
                {
                    var doc = htmlWeb.Load($"{basePsnAddress}/{zone}/category/85448d87-aa7b-4318-9997-7d25f4d275a4/{pageNumber++}");
                    if (doc.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "ems-sdk-grid-paginator-next-page-btn" && (bool)d.Attributes["class"]?.Value.Contains("psw-is-disabled")).Any())
                    {
                        isLastPage = true;
                    }

                    var gameLinks = doc.DocumentNode.Descendants().Where(d => d.Attributes["class"]?.Value == "ems-sdk-product-tile-link");

                    foreach (var link in gameLinks)
                    {
                        string gameUrl = basePsnAddress + link.Attributes["href"].Value;
                        var gamePage = htmlWeb.Load(gameUrl);
                        string gameName = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfe-game-title#name").FirstOrDefault().InnerText;
                        string gameFinalPrice = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#finalPrice").FirstOrDefault().InnerText;
                        string gameOriginalPrice = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#originalPrice").FirstOrDefault()?.InnerText;
                        string gameDescountDescriptor = gamePage.DocumentNode.Descendants().Where(d => d.Attributes["data-qa"]?.Value == "mfeCtaMain#offer0#discountDescriptor").FirstOrDefault()?.InnerText;

                        Console.WriteLine($"{count++} - {gameName} - {gameFinalPrice} - {gameOriginalPrice} - {gameDescountDescriptor}");
                    }
                }
                while (!isLastPage);
            }
        }
    }
}
