using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Attributes;
using ProductCatalog.Model.Dto.CategoryDtos;
using ProductCatalog.Service.BusinessLogic.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [ApiResponse]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Lấy tất cả các Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // Lấy Category theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // Tạo mới Category
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDTO createCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryId }, createdCategory);
        }

        // Cập nhật Category
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateCategoryDTO updateCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateCategoryDTO.CategoryId)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await _categoryService.UpdateCategoryAsync(updateCategoryDTO);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Cập nhật thành công.");  // Cập nhật thành công
        }

        // Xóa Category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();  // Xóa thành công
        }

        // Lấy tất cả các category con theo category ID
        [HttpGet("{id}/children")]
        public async Task<IActionResult> GetCategoryChildrenById(int id)
        {
            var children = await _categoryService.GetCategoryChildrenById(id);
            if (children == null || children.Count == 0)
            {
                return NotFound();
            }

            return Ok(children);
        }
    }
}
