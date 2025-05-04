using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Post;

namespace PixelIt.DTOs.Like
{
    public class CreateLikeDto
    {
        [Required]
        public DateTime DateLike { get; set; } 
        public bool IsLike { get; set; }
        public int LikeCount { get; set; }
        public UserPostDto User { get; set; }
        public PostCommentDto Post { get; set; }


    }
}
