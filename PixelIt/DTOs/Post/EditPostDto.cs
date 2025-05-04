using PixelIt.DTOs.PostCategory;
using PixelIt.Models;
using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Post
{
    public class EditPostDto
    {

        
        public string? PostImage { get; set; }

        public IFormFile? NewPostImage { get; set; }

        public DateTime PostDate { get; set; }

        public bool UpdateDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        public virtual ICollection<PostCategorySimpleDto> PostCategories { get; set; }


    }
}
