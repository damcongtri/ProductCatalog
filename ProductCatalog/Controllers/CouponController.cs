using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Model.Dto.CouponDtos;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCouponForShop(int shopId)
        {
            return Ok();
        }
        [HttpPost("apply-coupon")]
        public IActionResult ApplyCoupon(string couponCode)
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult AddCoupon(CreateCouponDto couponDto)
        {
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteCoupon(int couponId)
        {
            return Ok();
        }
        [HttpPut]
        public IActionResult UpdateCoupon(UpdateCouponDto couponDto)
        {
            return Ok();
        }
    }
}
