using System.ComponentModel.DataAnnotations;

namespace PixelIt.DTOs.Account
{
    public class UserPostDto
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Nickname { get; set; }

        public string ProfilePicture { get; set; }


    }
}
