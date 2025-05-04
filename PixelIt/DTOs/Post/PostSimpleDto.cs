using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;

namespace PixelIt.DTOs.Post
{
    public class PostSimpleDto
    {

        [Required]
        public required string PostImage { get; set; }

        public string? NewPostImage { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required DateTime PostDate { get; set; }

        public bool UpdateDate { get; set; }

        public virtual ICollection<PostCategorySimpleDto> PostCategories { get; set; }

        public virtual ICollection<LikeSimpleDto> Likes { get; set; }

        public virtual ICollection<CommentSimpleDto> Comments { get; set; }

        [Required]
        public required string Nickname { get; set; }

        [Required]
        public required string ProfilePicture { get; set; }

        [Required]
        public UserPostDto User { get; set; }

    }
}
