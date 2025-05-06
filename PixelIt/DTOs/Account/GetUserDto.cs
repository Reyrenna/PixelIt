using PixelIt.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.ImageCollection;
using PixelIt.DTOs.Post;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Follow;

namespace PixelIt.DTOs.Account
{
    public class GetUserDto
    {
        [Required]
        public string IdUser { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Nickname { get; set; }

        public string? ProfileDescription { get; set; }

        [Required]
        public required string ProfilePicture { get; set; }

        [Required]
        public required DateOnly DateOfBirth { get; set; }

        [Required]
        public required DateTime DateOfRegistration { get; set; }

        public virtual ICollection<ImageCollectionSimpleDto>? ImageCollections { get; set; }

        public virtual ICollection<PostSimpleDto>? Posts { get; set; }

        public virtual ICollection<CommentSimpleDto>? Comments { get; set; }

        public virtual ICollection<LikeSimpleDto>? Likes { get; set; }

        public virtual ICollection<FollowSimpleDto> Followings { get; set; }

        public virtual ICollection<FollowSimpleDto> Followers { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
