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
        [Route("{gameName?}")]
        public IActionResult Get(string gameName = "", int? userId = null)
        {
            var playstationGames = string.IsNullOrEmpty(gameName)
                ? _playstationWishlistDbContext.PlaystationGames.Where(g => g.OriginalPrice != null && g.Region == "en-US").Take(50).OrderBy(g => g.Name)
                : _playstationWishlistDbContext.PlaystationGames.Where(g => g.Name.Contains(gameName) && g.Region == "en-US").Take(50).OrderBy(g => g.Name);

            var mappedPlaystationGames = _mapper.Map<List<PlaystationGame>>(playstationGames);
            if (userId != null)
            {
                var gamesOnUserWishlist =
                    _playstationWishlistDbContext.WishlistItems.Where(w => w.UserId == userId.Value);

                mappedPlaystationGames = mappedPlaystationGames.Select(psnGame =>
                {
                    psnGame.IsOnUserWishlist =
                        gamesOnUserWishlist.Any(wishlistItem => wishlistItem.GameUrl == psnGame.Url);
                    return psnGame;
                }).ToList();
            }

            return Ok(mappedPlaystationGames);
        }
    }
}
