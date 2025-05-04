using PixelIt.DTOs.Account;
using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Follow
{
    public class CreateFollowDto
    {
        [Required]
        public DateTime DateFollow { get; set; }

        public UserSimpleDto? Follower { get; set; }

        public UserSimpleDto? Followed { get; set; }

    }
}
