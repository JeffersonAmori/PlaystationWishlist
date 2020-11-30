using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PlaystationGamesImporterWebJob
{
    // ReSharper disable once UnusedMember.Global
    public class Functions
    {
        private readonly IMapper _mapper;

        public Functions()
        {
            _mapper = (IMapper)ServiceLocator.Instance.GetService(typeof(IMapper));
        }

        // ReSharper disable once UnusedMember.Global
        public async Task ProcessTimerAction([TimerTrigger("0 0 * * * *", RunOnStartup = true)] TimerInfo timerInfo, ILogger logger)
        {
            try
            {
                var dbContext = (PlaystationWishlistContext)ServiceLocator.Instance.GetService(typeof(PlaystationWishlistContext));
                var regions = new[] { "en-US", "pt-BR" };

                dbContext.PlaystationGames.RemoveRange(dbContext.PlaystationGames);

                foreach (var region in regions)
                {
                    foreach (var game in await PlaystationStoreScrapper.GetAllGames(region))
                    {
                        await dbContext.PlaystationGames.AddAsync(_mapper.Map<PlaystationWishlist.DataAccess.Models.PlaystationGame>(game));
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ProcessTimerAction error.");
                throw;
            }
        }
    }
}