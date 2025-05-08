using Microsoft.AspNetCore.Mvc;
using PixelIt.DTOs.Category;
using PixelIt.Services;

namespace PixelIt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _category;

        public CategoryController(CategoryService category)
        {
            _category = category;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _category.GetCategories();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Invalid category data.");
            }
            var createdCategory = _category.CreateCategory(category);
            return CreatedAtAction(nameof(GetCategories), createdCategory);
        }

        [HttpDelete]
        public IActionResult DeleteCategory(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid category ID.");
            }
            var deletedCategory = _category.DeleteCategory(id);
            return NotFound("Category not found.");
        }
    }



}
