using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlistWebSite.Models;
using PlaystationWishlistWebSite.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                LocalRedirect(Url.Action("Index", "Home"));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, Route("[controller]/ExternalLogin")]
        public IActionResult ExternalLogin(string returnUrl = null, string provider = "Google")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var redirectUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = redirectUrl,
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provides: {remoteError}");
                return View("Login", loginViewModel);
            }

            var externalInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalInfo == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information.");
                return View("Login", loginViewModel);
            }

            var signinResult = await _signInManager.ExternalLoginSignInAsync(externalInfo.LoginProvider,
                externalInfo.ProviderKey, isPersistent: true);

            if (signinResult.Succeeded)
            {
                return LocalRedirect(redirectUrl);
            }
            else
            {
                var email = externalInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new AppUser()
                        {
                            Name = externalInfo.Principal.FindFirstValue(ClaimTypes.Name),
                            UserName = externalInfo.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = externalInfo.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, externalInfo);
                    await _signInManager.SignInAsync(user, isPersistent: true);

                    return LocalRedirect(redirectUrl);
                }
            }

            return View("Login", loginViewModel);
        }
    }
}
