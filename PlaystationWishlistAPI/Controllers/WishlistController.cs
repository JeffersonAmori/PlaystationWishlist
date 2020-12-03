using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaystationWishlist.DataAccess.Data;
using PlaystationWishlist.DataAccess.Models;
using PlaystationWishlistAPI.Models;

namespace PlaystationWishlistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly PlaystationWishlistContext _playstationWishlistContext;
        private readonly IMapper _mapper;

        public WishlistController(PlaystationWishlistContext playstationWishlistContext, IMapper mapper)
        {
            _playstationWishlistContext = playstationWishlistContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("userId:int")]
        public async Task<IActionResult> Get(int userId)
        {
            return Ok(_mapper.Map<List<PlaystationWishlist.Core.Entities.WishlistItem>>(
                _playstationWishlistContext.WishlistItems.Where(w => w.UserId == userId)));
        }

        [HttpPost]
        public async Task<IActionResult> Post(InNewWishlistItem inNewWishlistItem)
        {
            await _playstationWishlistContext.AddAsync(
                _mapper.Map<WishlistItem>(inNewWishlistItem));

            await _playstationWishlistContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        //[Route("{userId:int}/{gameUrl}")]
        public async Task<IActionResult> Delete(int userId, string gameUrl)
        {
            var itemsToDelete = _playstationWishlistContext.WishlistItems.Where(x =>
                x.UserId == userId && x.GameUrl == gameUrl);

            if (!itemsToDelete.Any())
                return NotFound();

            _playstationWishlistContext.WishlistItems.RemoveRange(itemsToDelete);
            await _playstationWishlistContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
