using PixelIt.DTOs.Account;

namespace PixelIt.DTOs.Follow
{
    public class GetFollowDto
    {
        public DateTime DateFollow { get; set; }
        public UserSimpleDto? Follower { get; set; }
        public UserSimpleDto? Followed { get; set; }
    }
}
