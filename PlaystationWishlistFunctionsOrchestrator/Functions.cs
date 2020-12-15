using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using PlaystationWishlist.EmailSender;

namespace PlaystationWishlistFunctionsOrchestrator
{

    public class Functions
    {
        private readonly IMapper _mapper;
        private readonly PlaystationWishlistContext _playstationWishlistContext;
        private readonly IdentityAppContext _identityAppContext;
        private readonly List<Task> _tasks = new List<Task>();

        public Functions(IMapper mapper, PlaystationWishlistContext playstationWishlistContext, IdentityAppContext identityAppContext)
        {
            _mapper = mapper;
            _playstationWishlistContext = playstationWishlistContext;
            _identityAppContext = identityAppContext;
        }

        [FunctionName("UpdateStoreAndSendEmail_OnScheduleStart")]
        public async Task OnScheduleStart(
            [TimerTrigger("0 0 * * *", RunOnStartup = true)] TimerInfo timerInfo,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var regions = new[] { "en-US"/*, "pt-BR"*/ };

            try
            {
                foreach (var region in regions)
                {
                    _playstationWishlistContext.PlaystationGames.RemoveRange(_playstationWishlistContext.PlaystationGames.Where(g => g.Region == region));

                    foreach (var game in await PlaystationStoreScrapper.GetAllGamesByRegion(region))
                    {
                        await _playstationWishlistContext.PlaystationGames.AddAsync(_mapper.Map<PlaystationWishlist.DataAccess.Models.PlaystationGame>(game));
                    }

                    Console.WriteLine("Saving changes to database.");
                    await _playstationWishlistContext.SaveChangesAsync();

                    Console.WriteLine("Preparing to send e-mail.");
                    var discountedGames = _playstationWishlistContext.PlaystationGames.Where(g => g.OriginalPrice != null);

                    foreach (var discountedGame in discountedGames.ToList())
                    {
                        discountedGame.DiscountPercentage = CalculateDiscountPercentage(discountedGame);

                        var wishlistItemsForGame = _playstationWishlistContext.WishlistItems.Where(item => item.GameUrl == discountedGame.Url);

                        foreach (var wishlistItem in wishlistItemsForGame)
                        {
                            var user = _identityAppContext.Users.FirstOrDefault(u => u.Id == wishlistItem.UserId);
                            if (user != null)
                            {
                                Console.WriteLine($"Sending e-mail to {user.Email} about {discountedGame.Name}");
                                Sender.Send(discountedGame, user);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "ProcessTimerAction error.");
                throw;
            }

            Task.WaitAll(_tasks.ToArray());
        }
        private double? CalculateDiscountPercentage(PlaystationWishlist.DataAccess.Models.PlaystationGame game) =>
            Convert.ToDouble(((game.FinalPrice - game.OriginalPrice) - game.OriginalPrice) * 100);
    }
}