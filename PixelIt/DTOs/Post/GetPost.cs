using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;

namespace PixelIt.DTOs.Post
{
    public class GetPost
    {

        [Required]
        public Guid IdPost { get; set; }

        [Required]
        public required string PostImage { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required DateTime PostDate { get; set; }

        public virtual ICollection<PostCategorySimpleDto> PostCategories { get; set; }

        public virtual ICollection<LikeSimpleDto> Likes { get; set; }

        public virtual ICollection<CommentSimpleDto> Comments { get; set; }

        [Required]
        public string IdUser { get; set; }

        [Required]
        public UserPostDto User { get; set; }

    }
}
