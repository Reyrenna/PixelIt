using PixelIt.DTOs.Account;


namespace PixelIt.DTOs.Like
{
    public class GetLikeDto
    {
        public int LikeCount { get; set; }
        public DateTime LikeDate { get; set; }
        public UserPostDto User { get; set; }
    }
}
