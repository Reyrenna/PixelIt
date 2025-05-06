using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using PixelIt.DTOs.Account;

namespace PixelIt.DTOs.ImageCollection
{
    public class ImageCollectionSimpleDto
    {
        public Guid IdImageCollection { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }
        public IFormFile Image4 { get; set; }
        public IFormFile Image5 { get; set; }

        [Required]
        public required string IdUser { get; set; }  
        public UserSimpleDto user { get; set; }

    }
}
