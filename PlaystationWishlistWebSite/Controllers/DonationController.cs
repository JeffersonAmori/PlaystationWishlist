using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite.Controllers
{
    public class DonationController : Controller
    {
        public IActionResult Thanks()
        {
            return View();
        }
        public IActionResult Canceled()
        {
            return View();
        }
    }
}
