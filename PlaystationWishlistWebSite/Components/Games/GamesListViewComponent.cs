using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using PlaystationWishlist.DataAccess.Models.Identity;
using PlaystationWishlistWebSite.Models;

namespace PlaystationWishlistWebSite.Components.Games
{
    public class GamesListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public GamesListViewComponent(IHttpClientFactory clientFactory, IMapper mapper, UserManager<AppUser> userManager)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string gameName, bool showOnlyGamesOnWishlist = false)
        {
            var httpClient = _clientFactory.CreateClient();

            var gamesSearchUrl = Environment.GetEnvironmentVariable("PLAYSTATION_WISHLIST_API").TrimEnd('/') + "/api/games?gameName=" + gameName;

            if (User.Identity.IsAuthenticated)
            {
                int? userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
                gamesSearchUrl += $"&userId={userId}";

                if(showOnlyGamesOnWishlist)
                    gamesSearchUrl += $"&onlyGamesOnWishlist={showOnlyGamesOnWishlist}";
            }

            var response = await httpClient.GetAsync(gamesSearchUrl);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            var model = new GamesListViewModel
            {
                GamesViewModels = _mapper.Map<List<GamesViewModel>>(result),
                ShowSearchBar = !showOnlyGamesOnWishlist
            };

            return View(model);
        }
    }
}
