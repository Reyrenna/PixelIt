using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelIt.Models
{
    public class Category
    {
        [Key]
        public Guid IdCategory { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public ICollection<PostCategory> PostCategories { get; set; } 

    }
}
