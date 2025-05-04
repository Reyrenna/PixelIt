namespace PixelIt.DTOs.Account
{
    public class EditUserDto
    {

        public string? Nickname { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? ProfileDescription { get; set; }
    }
}
