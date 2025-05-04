using Microsoft.AspNetCore.Mvc;
using PixelIt.DTOs.Account;
using PixelIt.Services;

namespace PixelIt.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ImageCollectionService _imageCollectionService;

        public UserController(UserService userService, ImageCollectionService imageCollectionService)
        {
            _userService = userService;
            _imageCollectionService = imageCollectionService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> RegistrationUser(CreateUserDto createUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _userService.CreateUserAsync(createUser);

                    var imageCollectionResult = await _imageCollectionService.CreateImageCollection((DTOs.ImageCollection.ImageCollectionSimpleDto)createUser.ImageCollections);
                    if (imageCollectionResult)
                    {
                        return Ok(new { message = "User and image collection created successfully." });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to create image collection." });
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the user.", error = ex.Message });
            }
        }
    }
}
