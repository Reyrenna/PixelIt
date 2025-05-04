using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Category;

namespace PixelIt.DTOs.PostCategory
{
    public class PostCategorySimpleDto
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public ICollection<GetCategoriesDto> Category { get; set; }
    }
}
