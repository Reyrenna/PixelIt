using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(80, ErrorMessage = "Category name cannot be longer than 80 characters.")]
        public string CategoryName { get; set; }
    }
}
