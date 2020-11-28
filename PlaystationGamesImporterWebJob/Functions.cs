using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaystationGamesImporterWebJob
{
    public class Functions
    {
        private IMapper _mapper;

        public Functions()
        {
            _mapper = (IMapper)ServiceLocator.Instance.GetService(typeof(IMapper));
        }

        public async Task ProcessTimerAction([TimerTrigger("0 * * * *", RunOnStartup = true)] TimerInfo timerInfo, ILogger logger)
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
                        dbContext.PlaystationGames.Add(_mapper.Map<PlaystationWishlist.DataAccess.Models.PlaystationGame>(game));
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