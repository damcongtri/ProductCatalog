using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Model.Dto.WishlistDtos;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        [HttpGet("report-wishlist")]
        public IActionResult GetAllWishlists()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetWishlistById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddWishlist([FromBody] CreateWishlistDto wishlistDto)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWishlist(int id)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetWishlistByUserId(int userId)
        {
            return Ok();
        }
    }
}
