using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelIt.DTOs.Account;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class UserService : IdentityUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserService> _logger;
        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UserService> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task<IdentityResult> CreateUserAsync(
            ApplicationUser user, 
            string password,
            IFormFile profilePicture,
            string roleName
            )
        {
            try
            {
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    var fileName = profilePicture.FileName;
                    var UniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile images", UniqueFileName);

                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(stream);
                    }

                    var webRootPath = Path.Combine("uploads", "profile images", UniqueFileName);
                }
                else
                {
                    user.ProfilePicture = Path.Combine("uploads", "profile images", "default.png");
                }

                user.DateOfRegistration = DateTime.UtcNow;

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    //RICOEDATI DI CAMBIARE IL RUOLO DI DEFAULT
                    await _userManager.AddToRoleAsync(user, "DefaultUser");
                    _logger.LogInformation($"User {user.UserName} created successfully.");
                }
                else
                {
                    _logger.LogError($"Error creating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user {user.UserName}: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }


        }

        public async Task<ApplicationUser> GetUserByNicknameAsync(string nickname)
        {
            return await _userManager.Users.OfType<ApplicationUser>().FirstOrDefaultAsync(u => u.Nickname == nickname);
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<IdentityResult> EditUserAsync(ApplicationUser appUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(appUser.Id);
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });
                }
                appUser.Nickname = appUser.Nickname;
                appUser.ProfilePicture = appUser.ProfilePicture;
                appUser.PhoneNumber = appUser.PhoneNumber;
                appUser.ProfileDescription = appUser.ProfileDescription;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {user.UserName} updated successfully.");
                }
                else
                {
                    _logger.LogError($"Error updating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user: {ex.Message}");
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        internal async Task CreateUserAsync(CreateUserDto createUser)
        {
            throw new NotImplementedException();
        }
    }
}
