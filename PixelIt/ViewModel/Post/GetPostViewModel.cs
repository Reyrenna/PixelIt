using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Post;

namespace PixelIt.ViewModel.Post
{
    public class GetPostViewModel
    {
        [Required]
        public Guid IdPost { get; set; }

        [Required]
        public required string PostImage { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]

        public string ProfileImage { get; set; }

        [Required]
        public string IdUser { get; set; }

        [Required]
        public UserPostDto User { get; set; }
    }
}
