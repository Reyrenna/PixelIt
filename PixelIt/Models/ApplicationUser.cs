//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Identity.Client;

//namespace PixelIt.Models
//{
//    public class ApplicationUser : IdentityUser
//    {
//        [Required(ErrorMessage = "Il nome è obbligatorio")]
//        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
//        public string Name { get; set; }

//        [Required(ErrorMessage = "Il cognome è obbligatorio")]
//        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
//        public string Surname { get; set; }

//        [Required(ErrorMessage = "Il nickname è obbligatorio")]
//        [StringLength(30, ErrorMessage = "Il nickname non può superare i 30 caratteri")]
//        public string Nickname { get; set; }

//        [Required(ErrorMessage = "L'email è obbligatoria")]
//        [EmailAddress(ErrorMessage = "Formato email non valido")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "La password è obbligatoria")]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "La password deve essere lunga almeno 6 caratteri")]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [Required(ErrorMessage = "La conferma della password è obbligatoria")]
//        [Compare("Password", ErrorMessage = "La password e la conferma password non corrispondono")]
//        [DataType(DataType.Password)]
//        public string ConfirmPassword { get; set; }

//        public string? ProfileDescription { get; set; }

//        [Required]
//        public required string ProfilePicture { get; set; }

//        [Required]
//        public required DateOnly DateOfBirth { get; set; }

//        [Required]
//        public required DateTime DateOfRegistration { get; set; }

//        public virtual ICollection<ImageCollection>? ImageCollections { get; set; }

//        public virtual ICollection<Post>? Posts { get; set; }

//        public virtual ICollection<Comment>? Comments { get; set; }

//        public virtual ICollection<Like>? Likes { get; set; }

//        public virtual ICollection<Follow> Followings { get; set; } = new List<Follow>();

//        public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();

//        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

//    }
//}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PixelIt.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Il nickname è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il nickname non può superare i 30 caratteri")]
        public string Nickname { get; set; }

        public string? ProfileDescription { get; set; }

        public string? ProfilePicture { get; set; }

        public string? VerificationImage1 { get; set; }

        public string? VerificationImage2 { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; } // Cambiato da DateOnly a DateTime per compatibilità

        [Required]
        public DateTime DateOfRegistration { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Follow> Followings { get; set; } = new List<Follow>();
        public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        // Rimossi Password e ConfirmPassword che sono gestiti da IdentityUser
        // Rimosso Email duplicato perché è già presente in IdentityUser
    }
}