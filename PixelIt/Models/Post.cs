using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelIt.Models
{
    public class Post
    {
        [Key]
        public Guid IdPost { get; set; }

        [Required] 
        public string Description { get; set; }

        [Required]
        public DateTime PostDate { get; set; }

        public bool UpdateDate { get; set; }

        [Required]
        
        public string PostImage { get; set; }

        public string NewPostImage { get; set; }

        public virtual ICollection<PostCategory>? PostCategories { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty("Posts")]
        public string IdUser { get; set; }
        public ApplicationUser? User { get; set; }

        public virtual ICollection<Like>? Likes { get; set; } 

        public virtual ICollection<Comment>? Comments { get; set; }

    }
}
