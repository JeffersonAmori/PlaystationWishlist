using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using PlaystationWishlist.Core.Entities;

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
        public IActionResult Get(string gameName = "")
        {
            var playstationGames = string.IsNullOrEmpty(gameName)
                ? _playstationWishlistDbContext.PlaystationGames.Where(g => g.OriginalPrice != null && g.Region == "en-US").OrderBy(g => g.Name)
                : _playstationWishlistDbContext.PlaystationGames.Where(g => g.Name.Contains(gameName));

            return Ok(
                _mapper.Map<List<PlaystationGame>>(
                    playstationGames));
        }
    }
}
