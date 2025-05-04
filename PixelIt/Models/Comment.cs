using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelIt.Models
{
    public class Comment
    {
        [Key]
        public Guid IdComment { get; set; }

        [Required]
        public string CommentText { get; set; }

        [Required]
        public DateTime CommentDate { get; set; }

        public Guid IdPost { get; set; }
        [Required]
        [ForeignKey(nameof(IdPost))]
        [InverseProperty("Comments")]
        public Post? Post { get; set; }

        public string IdUser { get; set; }
        [Required]
        [ForeignKey(nameof(IdUser))]
        [InverseProperty("Comments")]
        public ApplicationUser? User { get; set; }

    }
}
