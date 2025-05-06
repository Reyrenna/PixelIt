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
        public Guid IdPost { get; set; }

        public string UserId { get; set; }

        public UserPostDto User { get; set; }

        public PostCommentDto Post { get; set; }
    }
}