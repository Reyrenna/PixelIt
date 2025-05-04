using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Follow;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Post;

namespace PixelIt.DTOs.Account
{
    public class UserSimpleDto
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public string ProfilePicture { get; set; }

        public virtual ICollection<PostSimpleDto> Posts { get; set; }

        public virtual ICollection<CommentSimpleDto> Comments { get; set; }

        public virtual ICollection<LikeSimpleDto> Likes { get; set; }

        public virtual ICollection<FollowSimpleDto> Followings { get; set; }

        public virtual ICollection<FollowSimpleDto> Followers { get; set; }

    }
}
