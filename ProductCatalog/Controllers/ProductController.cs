using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Attributes;
using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.ProductDtos;
using ProductCatalog.Service.BusinessLogic.Interfaces;
using System.Linq.Expressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiResponse]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParamsDto queryParameters)
        {
            var products = await _productService.GetProductsAsync(queryParameters);
            return Ok(products);
        }

        //POST: api/Product
        [HttpPost]
        //[ValidateModel]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProductWithVariants([FromForm] AddProductDto addProductDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //return Ok();
            try
            {
                var product = await _productService.AddProductWithVariantsAsync(addProductDto);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        //GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id); // Assume this method exists in ProductService
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //// DELETE: api/Product/variant/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("Product")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProduct(productDto);
                if (updatedProduct)
                {
                    return Ok(await _productService.GetProductByIdAsync(productDto.ProductId));
                }
                return BadRequest("update failed");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Product/variant
        //[HttpPut("variant")]
        //public async Task<IActionResult> UpdateProductVariant([FromBody] ProductVariantDto productVariantDto)
        //{
        //    try
        //    {
        //        var updatedVariant = await _productService.UpdateProductVariantAsync(productVariantDto);
        //        return Ok(updatedVariant);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        // POST: api/Product/attribute
        [HttpPost("attribute")]
        public async Task<IActionResult> AddVariantAttribute([FromBody] AddAttributeDto variantAttributeDto)
        {
            try
            {
                var addedAttribute = await _productService.AddVariantAttributeAsync(variantAttributeDto);
                return Ok(addedAttribute);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("attribute/{productId}")]

        public async Task<IActionResult> GetProductAttributesByProductId(int productId)
        {
            var data = await _productService.GetProductAttributesAsync(productId);
            return Ok(data);
        }

        // POST: api/Product/attribute/value
        [HttpPost("attribute/value")]
        public async Task<IActionResult> AddVariantAttributeValue([FromBody] AddAttributeValueDto variantAttributeValue)
        {
            try
            {
                var addedAttributeValue = await _productService.AddVariantAttributeValueAsync(variantAttributeValue);
                return Ok(addedAttributeValue);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Product/attributes
        [HttpGet("attributes")]
        public async Task<IActionResult> GetVariantAttributes([FromQuery]string? keywork, [FromQuery] int? id)
        {
            try
            {
                
                var attributes = await _productService.FindVariantAttributesAsync(keywork, id);
                return Ok(attributes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Product/attribute/{id}
        [HttpGet("Shop/Product/{id}")]
        public async Task<IActionResult> GetProductForShop(int id)
        {
            try
            {
                var attribute = await _productService.GetProductsForShopAsync(shopId: id);
                return Ok(attribute);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //// GET: api/Product/attribute-values
        //[HttpGet("attribute-values")]
        //public async Task<IActionResult> GetVariantAttributeValues([FromQuery] Expression<Func<VariantAttributeValue, bool>> predicate)
        //{
        //    try
        //    {
        //        var attributeValues = await _productService.FindVariantAttributeValuesAsync(predicate);
        //        return Ok(attributeValues);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //// POST: api/Product/attribute/upsert
        //[HttpPost("attribute/upsert")]
        //public async Task<IActionResult> UpsertVariantAttribute([FromBody] VariantAttribute variantAttribute)
        //{
        //    try
        //    {
        //        var result = await _productService.UpsertVariantAttributeAsync(variantAttribute);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //// POST: api/Product/attribute-value/upsert
        //[HttpPost("attribute-value/upsert")]
        //public async Task<IActionResult> UpsertVariantAttributeValue([FromBody] VariantAttributeValue variantAttributeValue)
        //{
        //    try
        //    {
        //        var result = await _productService.UpsertVariantAttributeValueAsync(variantAttributeValue);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
    }
}
