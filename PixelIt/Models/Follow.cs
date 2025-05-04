using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelIt.Models
{
    public class Follow
    {
        [Key]
        public Guid IdFollow { get; set; }

        [Required]
        public DateTime DateFollow { get; set; }

        [Required]
        public string IdFollowed { get; set; }

        [Required]
        public string IdFollower { get; set; }

        [ForeignKey(nameof(IdFollower))]
        [InverseProperty("Followers")]
        public ApplicationUser? Follower { get; set; }

        [ForeignKey(nameof(IdFollowed))]
        [InverseProperty("Followings")]
        public ApplicationUser? Followed { get; set; }
       
    }
}
