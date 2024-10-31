using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Attributes;
using ProductCatalog.Model.Dto.CartDtos;
using ProductCatalog.Service.BusinessLogic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [ApiResponse]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto cartDto)
        {
            var cart = await _cartService.AddCart(cartDto);
            return CreatedAtAction(nameof(GetCart), new { cartId = cart.CartId }, cart);
            //return Ok();
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            try
            {
                var cart = await _cartService.DeleteCart(cartId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
{
                return NotFound(ex.Message);
            }
        }

        // Lấy giỏ hàng theo cartId
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int cartId)
        {
            try
            {
                var cart = await _cartService.GetCart(cartId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Lấy tất cả sản phẩm trong giỏ hàng của người dùng
        [HttpGet("user")]
        public async Task<IActionResult> GetCartByUserId( int userId)
        {
            var carts = await _cartService.GetCartByUserId(userId);
            return Ok(carts);
        }

        // Cập nhật giỏ hàng
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart([FromBody] CartUpdateDto cartDto)
        {
            try
            {
                var cart = await _cartService.UpdateCart(cartDto);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Phương thức giả định để lấy UserId từ token
        //private int GetUserId()
        //{
        //    // Logic để lấy UserId từ claims hoặc session
        //    return int.Parse(User.FindFirst("UserId")?.Value);
        //}
    }
}
