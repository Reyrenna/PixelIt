using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Post
{
    public class PostCommentDto
    {
        [Required]
        public Guid IdPost { get; set; }
    }
}
