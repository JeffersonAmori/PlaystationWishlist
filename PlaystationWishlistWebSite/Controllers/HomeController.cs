using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlaystationWishlistWebSite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;

namespace PlaystationWishlistWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index([FromServices] IHttpClientFactory httpClientFactory)
        {
            var httpClient = httpClientFactory.CreateClient();
            
            var response = await httpClient.GetAsync(Environment.GetEnvironmentVariable("PlaystationWishlistAPI") + "/api/games");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            var model = _mapper.Map<List<GamesViewModel>>(result);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromServices] IHttpClientFactory httpClientFactory, object o)
        {
            var httpClient = httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(Environment.GetEnvironmentVariable("PlaystationWishlistAPI") + "/api/games/z");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            var model = _mapper.Map<List<GamesViewModel>>(result);

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
