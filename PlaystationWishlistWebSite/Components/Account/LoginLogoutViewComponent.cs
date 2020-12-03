using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlistWebSite.Models;
using System.Threading.Tasks;
using PlaystationWishlist.DataAccess.Models.Identity;

namespace PlaystationWishlistWebSite.Components.Account
{
    public class LoginLogoutViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public LoginLogoutViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            LoginLogoutViewModel loginLogoutViewModel = new LoginLogoutViewModel();

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                loginLogoutViewModel.LoggedInUserName = (await _userManager.GetUserAsync(HttpContext.User)).Name
                    .Split(" ").FirstOrDefault();
            }

            return View(loginLogoutViewModel);
        }
    }
}
