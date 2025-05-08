using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Category;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;

namespace PixelIt.DTOs.Post
{
    public class CreatePostDto
    {
        [Required]
        public IFormFile PostImage { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        
        public virtual CreateCategoryDto? PostCategories { get; set; }

        public virtual ICollection<GetCategoriesDto>? Categories { get; set; }

        [Required]
        public UserPostDto User { get; set; } 

    }
}
