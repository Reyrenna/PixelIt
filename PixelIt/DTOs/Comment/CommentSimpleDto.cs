using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Post;

namespace PixelIt.DTOs.Comment
{
    public class CommentSimpleDto
    {
        [Required]
        public Guid IdComment { get; set; }

        [Required]
        public string CommentText { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        [Required]
        public string IdPost { get; set; }

        [Required]
        public string UserId { get; set; }

        public required UserPostDto User { get; set; }

        public required PostCommentDto Post { get; set; }
    }
}