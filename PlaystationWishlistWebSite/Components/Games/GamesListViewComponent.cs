using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlistWebSite.Models;

namespace PlaystationWishlistWebSite.Components.Games
{
    public class GamesListViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public GamesListViewComponent(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(string gameName)
        {
            var httpClient = _clientFactory.CreateClient();

            var response = await httpClient.GetAsync(Environment.GetEnvironmentVariable("PLAYSTATION_WISHLIST_API") + "/api/games/" + gameName);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            var model = _mapper.Map<List<GamesViewModel>>(result);

            return View(model);
        }
    }
}
