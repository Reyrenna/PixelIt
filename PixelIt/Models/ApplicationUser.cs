using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace PixelIt.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Nickname { get; set; }

        public string? ProfileDescription { get; set; }

        [Required]
        public required string ProfilePicture { get; set; }

        [Required]
        public required DateOnly DateOfBirth { get; set; }

        [Required]
        public required DateTime DateOfRegistration { get; set; }

        public virtual ICollection<ImageCollection>? ImageCollections { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public virtual ICollection<Like>? Likes { get; set; }

        public virtual ICollection<Follow> Followings { get; set; } = new List<Follow>();

        public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
