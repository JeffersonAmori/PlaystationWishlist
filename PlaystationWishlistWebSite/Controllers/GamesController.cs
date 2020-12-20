using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlaystationWishlist.DataAccess.Models.Identity;
using PlaystationWishlistAPI.Models;
using PlaystationWishlistWebSite.Models;

namespace PlaystationWishlistWebSite.Controllers
{
    public class GamesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;

        public GamesController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<object> AddOrRemoveGameToWishList(string gameUrl, bool remove = false)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                {
                    return new { Status = "NOK", Message = "You must be logged in to add games to your wishlist." };
                }

                var httpClient = _httpClientFactory.CreateClient();

                var response = remove ?
                    await httpClient.DeleteAsync(
                    Environment.GetEnvironmentVariable("PLAYSTATION_WISHLIST_API").TrimEnd('/') + $"/api/wishlist?userId={(await _userManager.GetUserAsync(HttpContext.User)).Id}&gameUrl={HttpUtility.UrlEncode(gameUrl)}")
                    :
                    await httpClient.PostAsync(
                    Environment.GetEnvironmentVariable("PLAYSTATION_WISHLIST_API").TrimEnd('/') + "/api/wishlist", new StringContent(
                        JsonConvert.SerializeObject(new InNewWishlistItem()
                        {
                            UserId = (await _userManager.GetUserAsync(HttpContext.User)).Id,
                            GameUrl = gameUrl
                        }
                ), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                    return new { Status = "OK" };
                else
                    return new { Status = "NOK", Message = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return new { Status = "NOK", Message = ex.Message };
            }
        }
    }
}
