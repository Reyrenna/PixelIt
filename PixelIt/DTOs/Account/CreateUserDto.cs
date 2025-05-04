using System.ComponentModel.DataAnnotations;
using PixelIt.DTOs.ImageCollection;
using PixelIt.Models;

namespace PixelIt.DTOs.Account
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public virtual ICollection<ImageCollectionSimpleDto>? ImageCollections { get; set; }
    }
}
