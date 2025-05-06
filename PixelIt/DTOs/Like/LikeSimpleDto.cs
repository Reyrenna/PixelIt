using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Post;

namespace PixelIt.DTOs.Like
{
    public class LikeSimpleDto
    {
        [Required]
        public Guid IdLike { get; set; }

        [Required]
        public DateTime LikeDate { get; set; }
        public bool IsLike { get; set; }
        public int LikeCount { get; set; }

        public string IdUser { get; set; }

        [Required]
        public Guid IdPost { get; set; }
        public UserPostDto User { get; set; }
        public PostSimpleDto Post { get; set; }

    }
}
