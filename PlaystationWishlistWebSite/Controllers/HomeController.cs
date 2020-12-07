using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlaystationWishlistWebSite.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewBag.Layout = "NoLayout";
            }
            return View();
        }

        public async Task<IActionResult> MyWishlist()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewBag.Layout = "NoLayout";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewBag.Layout = "NoLayout";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
