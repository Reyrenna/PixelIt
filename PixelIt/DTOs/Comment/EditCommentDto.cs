using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Comment
{
    public class EditCommentDto
    {
        [Required]
        public string CommentText { get; set; }
    }
}
