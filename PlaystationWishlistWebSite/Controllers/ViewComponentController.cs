using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaystationWishlistWebSite.Controllers
{
    public class ViewComponentController : Controller
    {
        public IActionResult GamesList(string gameName)
        {
            return ViewComponent("GamesList", new { GameName = gameName });
        }
    }
}
