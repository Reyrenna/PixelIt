using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Account;

namespace PixelIt.DTOs.Follow
{
    public class FollowSimpleDto
    {

        [Required]
        public DateTime DateFollow { get; set; }

        [Required]
        public string IdFollowed { get; set; }

        [Required]
        public string IdFollower { get; set; }

        public UserSimpleDto? Follower { get; set; }

        public UserSimpleDto? Followed { get; set; }

    }
}
