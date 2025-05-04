using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Category
{
    public class GetCategoriesDto
    {
        [Required]
        public Guid IdCategory { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}
