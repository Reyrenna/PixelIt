using PixelIt.DTOs.Account;
using PixelIt.DTOs.ImageCollection;

namespace PixelIt.ViewModel.Account
{
    public class AddAccountViewModel
    {
        public CreateUserDto CreateUser { get; set; } 

        public ImageCollectionSimpleDto ImageCollection { get; set; }
    }
}
