using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;
using PlaystationWishlist.DataAccess.Data;
using PlaystationWishlist.EmailSender;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task ProcessTimerAction([TimerTrigger("0 0 */6 * * *", RunOnStartup = true)] TimerInfo timerInfo, ILogger logger)
        {
            try
            {
                var playstationWishlistContext = (PlaystationWishlistContext)ServiceLocator.Instance.GetService(typeof(PlaystationWishlistContext));
                var identityContext = (IdentityAppContext)ServiceLocator.Instance.GetService(typeof(IdentityAppContext));

                var regions = new[] { "en-US"/*, "pt-BR"*/ };

                playstationWishlistContext.PlaystationGames.RemoveRange(playstationWishlistContext.PlaystationGames);

                foreach (var region in regions)
                {
                    foreach (var game in await PlaystationStoreScrapper.GetAllGamesByRegion(region))
                    {
                        await playstationWishlistContext.PlaystationGames.AddAsync(_mapper.Map<PlaystationWishlist.DataAccess.Models.PlaystationGame>(game));
                    }
                }

                var discountedGames = playstationWishlistContext.PlaystationGames.Where(g => g.OriginalPrice != null);

                foreach (var discountedGame in discountedGames.ToList())
                {
                    discountedGame.DiscountPercentage = CalculateDiscountPercentage(discountedGame);

                    var wishlistItemsForGame = playstationWishlistContext.WishlistItems.Where(item => item.GameUrl == discountedGame.Url);

                    foreach (var wishlistItem in wishlistItemsForGame)
                    {
                        var user = identityContext.Users.FirstOrDefault(u => u.Id == wishlistItem.UserId);
                        if (user != null)
                        {
                            Console.WriteLine($"Sending e-mail to {user.Email} about {discountedGame.Name}");
                            Sender.Send(discountedGame, user);
                        }
                    }
                }

                Console.WriteLine("Saving changes to database.");
                await playstationWishlistContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ProcessTimerAction error.");
                throw;
            }
        }

        private double? CalculateDiscountPercentage(PlaystationWishlist.DataAccess.Models.PlaystationGame game) =>
            Convert.ToDouble(((game.FinalPrice - game.OriginalPrice) - game.OriginalPrice) * 100);
    }
}