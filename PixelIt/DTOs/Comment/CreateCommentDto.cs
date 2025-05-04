using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public string CommentText { get; set; }
    }
}
