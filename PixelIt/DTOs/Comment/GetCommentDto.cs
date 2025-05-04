using PixelIt.DTOs.Account;
using PixelIt.DTOs.Post;
using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Comment
{
    public class GetCommentDto
    {
        [Required]
        public string CommentText { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        public required UserPostDto User { get; set; }

        public required PostCommentDto Post { get; set; }
    }
}
