using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Follow
{
    public class DeleteFollowDto
    {
        [Required]

        public string Message { get; set; } = "Follow deleted successfully";
    }
}
