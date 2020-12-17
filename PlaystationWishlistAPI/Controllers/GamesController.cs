using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlist.Core.Entities;
using PlaystationWishlist.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;

namespace PlaystationWishlistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly PlaystationWishlistContext _playstationWishlistDbContext;
        private readonly IMapper _mapper;

        public GamesController(PlaystationWishlistContext playstationWishlistDbContext, IMapper mapper)
        {
            _playstationWishlistDbContext = playstationWishlistDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(string gameName = "", int? userId = null, bool onlyGamesOnWishlist = false)
        {
            IQueryable<PlaystationWishlist.DataAccess.Models.PlaystationGame> playstationGames;
            List<PlaystationGame> mappedPlaystationGames = new List<PlaystationGame>();
            if (onlyGamesOnWishlist)
            {
                var gamesOnUserWishlist =
                    _playstationWishlistDbContext.WishlistItems.Where(w => w.UserId == userId.Value);


                playstationGames = _playstationWishlistDbContext.PlaystationGames.Where(psnGame =>
                        gamesOnUserWishlist.Any(wishlistItem => wishlistItem.GameUrl == psnGame.Url));

                mappedPlaystationGames = _mapper.Map<List<PlaystationGame>>(playstationGames);
                mappedPlaystationGames = mappedPlaystationGames.Select(psnGame =>
                {
                    psnGame.IsOnUserWishlist =
                        gamesOnUserWishlist.Any(wishlistItem => wishlistItem.GameUrl == psnGame.Url);
                    return psnGame;
                }).ToList();
            }
            else
            {
                var gamesOnUserWishlist =
                    _playstationWishlistDbContext.WishlistItems.Where(w => w.UserId == userId.Value);

                playstationGames = string.IsNullOrEmpty(gameName)
                    ? _playstationWishlistDbContext.PlaystationGames.Where(g => g.OriginalPrice != null && g.Region == "en-US").Take(20)
                    : _playstationWishlistDbContext.PlaystationGames.Where(g => g.Name.Contains(gameName ?? string.Empty) && g.Region == "en-US").Take(20);

                mappedPlaystationGames = _mapper.Map<List<PlaystationGame>>(playstationGames);
                mappedPlaystationGames = mappedPlaystationGames.Select(psnGame =>
                {
                    psnGame.IsOnUserWishlist =
                        gamesOnUserWishlist.Any(wishlistItem => wishlistItem.GameUrl == psnGame.Url);
                    return psnGame;
                }).ToList();
            }


            return Ok(mappedPlaystationGames.OrderBy(g => g.Name));
        }
    }
}
