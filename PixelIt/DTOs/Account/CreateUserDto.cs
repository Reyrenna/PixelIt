//using System.ComponentModel.DataAnnotations;
//using PixelIt.DTOs.ImageCollection;
//using PixelIt.Models;

//namespace PixelIt.DTOs.Account
//{
//    public class CreateUserDto
//    {
//        [Required]
//        public string Name { get; set; }

//        [Required]
//        public string Surname { get; set; }

//        [Required]
//        public string Email { get; set; }

//        [Required]
//        public string Nickname { get; set; }

//        [Required]
//        public IFormFile ProfilePicture { get; set; }

//        public string? ProfileDescription { get; set; }

//        [Required]
//        public required string Password { get; set; }

//        [Required]
//        public DateOnly DateOfBirth { get; set; }

//        public DateTime DateOfRegistration { get; set; } 

//        public string ConfirmPassword { get; set; }

//        public virtual ICollection<ImageCollectionSimpleDto>? ImageCollections { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PixelIt.DTOs.Account
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il nickname è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il nickname non può superare i 30 caratteri")]
        public string Nickname { get; set; }

        //[Required(ErrorMessage = "L'immagine del profilo è obbligatoria")]
        public IFormFile? ProfilePicture { get; set; }

        public string? ProfileDescription { get; set; }

        [Required(ErrorMessage = "La password è obbligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La password deve essere lunga almeno 6 caratteri")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La conferma della password è obbligatoria")]
        [Compare("Password", ErrorMessage = "La password e la conferma password non corrispondono")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "La data di nascita è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } 

        public IFormFile? VerificationImage1 { get; set; }
        public IFormFile? VerificationImage2 { get; set; }
    }
}