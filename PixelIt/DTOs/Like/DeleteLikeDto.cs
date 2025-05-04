using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Like
{
    public class DeleteLikeDto
    {
        [Required]

        public string Message { get; set; } = "Like deleted successfully";
    }
}
