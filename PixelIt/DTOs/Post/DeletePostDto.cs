using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Post
{
    public class DeletePostDto
    {

        [Required]
        public string Message { get; set; } = "Post deleted successfully";

    }
}
