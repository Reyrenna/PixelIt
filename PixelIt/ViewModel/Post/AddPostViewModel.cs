using PixelIt.DTOs.Category;
using PixelIt.DTOs.Post;

namespace PixelIt.ViewModel.Post
{
    public class AddPostViewModel
    {
       public CreatePostDto Post { get; set; } 

       public CreateCategoryDto Category { get; set; }
    }
}
