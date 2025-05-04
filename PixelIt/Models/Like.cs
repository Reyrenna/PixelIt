using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelIt.Models
{
    public class Like
    {
        [Key]
        public Guid IdLike { get; set; } 

        [Required]
        public DateTime LikeDate { get; set; } 

        public bool IsLike { get; set; }

        public int LikeCount { get; set; }

        [Required]
        [ForeignKey(nameof(IdPost))]
        [InverseProperty("Likes")]
        public Guid IdPost { get; set; }
        public Post? Post { get; set; }

        [Required]
        [ForeignKey(nameof(IdUser))]
        [InverseProperty("Likes")]
        public string IdUser { get; set; }
        public ApplicationUser? User { get; set; }

    }
}
