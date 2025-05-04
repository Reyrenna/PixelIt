using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PixelIt.Models
{
    public class ImageCollection
    {
        [Key]
        public Guid IdImageCollection { get; set; }

        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }

        [Required]
        public string IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty("ImageCollections")]
        public ApplicationUser? User { get; set; }


    }
}
