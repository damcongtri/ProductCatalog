using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Model.Dto.ReviewDto;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetReviewForProduct(int productId)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddReview(CreateReviewDto review)
        {   
            return Ok();
        }

    }
}
