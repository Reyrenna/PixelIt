//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;
//using PixelIt.Data;
//using PixelIt.DTOs.Account;
//using PixelIt.DTOs.Category;
//using PixelIt.DTOs.Comment;
//using PixelIt.DTOs.Follow;
//using PixelIt.DTOs.Like;
//using PixelIt.DTOs.Post;
//using PixelIt.DTOs.PostCategory;
//using PixelIt.Models;

//namespace PixelIt.Services
//{
//    public class UserService 
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<ApplicationRole> _roleManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly ILogger<UserService> _logger;
//        private readonly ApplicationDbContext _context;
//        public UserService(
//            UserManager<ApplicationUser> userManager,
//            RoleManager<ApplicationRole> roleManager,
//            SignInManager<ApplicationUser> signInManager,
//            ILogger<UserService> logger,
//            ApplicationDbContext context
//            )
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _signInManager = signInManager;
//            _logger = logger;
//            _context = context;
//        }
//        public async Task<(IdentityResult Result, ApplicationUser? User)> CreateUserAsync(
//            CreateUserDto userDto,
//            string password,
//            IFormFile profilePicture,
//            IFormFile verificationImage1,
//            IFormFile verificationImage2,
//            string roleName
//            )
//        {
            
//            try
//            {
//                if (verificationImage1 != null && verificationImage1.Length >0)
//                {
//                    User
//                    var fileName = verificationImage1.FileName;
//                    var UniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images collection", UniqueFileName);
//                    var directory = Path.GetDirectoryName(filePath);
//                    if (!Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }
//                    await using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await verificationImage1.CopyToAsync(stream);
//                    }

//                    var webRootPath = Path.Combine("uploads", "images collection", UniqueFileName);
//                    user.VerificationImage1 = webRootPath;
//                }

//                if (verificationImage2 != null && verificationImage2.Length > 0)
//                {
//                    var fileName = verificationImage2.FileName;
//                    var UniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images collection", UniqueFileName);
//                    var directory = Path.GetDirectoryName(filePath);
//                    if (!Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }
//                    await using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await verificationImage2.CopyToAsync(stream);
//                    }
//                    var webRootPath = Path.Combine("uploads", "images collection", UniqueFileName);
//                    user.VerificationImage2 = webRootPath;
//                }

//                if (profilePicture != null && profilePicture.Length > 0)
//                {
//                    var fileName = profilePicture.FileName;
//                    var UniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile images", UniqueFileName);

//                    var directory = Path.GetDirectoryName(filePath);
//                    if (!Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }

//                    await using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await profilePicture.CopyToAsync(stream);
//                    }

//                    var webRootPath = Path.Combine("uploads", "profile images", UniqueFileName);
//                    user.ProfilePicture = webRootPath;
//                }
//                else
//                {
//                    user.ProfilePicture = Path.Combine("uploads", "profile images", "default.png");
//                }
                
//                var existingUser = await _userManager.FindByEmailAsync(user.Email);
//                if (existingUser != null)
//                {


//                    var newUser = new ApplicationUser
//                    {
//                        UserName = userDto.Email,
//                        Email = userDto.Email,
//                        Name = userDto.Name,
//                        Surname = userDto.Surname,
//                        Nickname = userDto.Nickname,
//                        ProfileDescription = userDto.ProfileDescription,
//                        ProfilePicture = userDto.ProfilePicture,
//                        DateOfBirth = userDto.DateOfBirth,
//                        DateOfRegistration = DateTime.UtcNow,
//                        VerificationImage1 = userDto.VerificationImage1,
//                        VerificationImage2 = userDto.VerificationImage2,
//                        EmailConfirmed = true,
//                    };

//                    var UserByEmail = await _userManager.FindByEmailAsync(user.Email);

//                    if (UserByEmail.Email == newUser.Email && user != null)
//                    {
//                        await _userManager.AddToRoleAsync(user, "User");
//                    }
//                }
//                else
//                {
//                    _logger.LogError($"The email {existingUser.Email} is already in use.");
//                }

//                    var result = await _userManager.CreateAsync(user, password);

//                if (result.Succeeded)
//                {
//                    _logger.LogInformation($"User {user.UserName} created successfully.");
//                }
//                else
//                {
//                    _logger.LogError($"Error creating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//                }
               
//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error creating user {user.UserName}: {ex.Message}");
//                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
//            }

//        }

//        public async Task<List<GetUserDto>> GetAllUser()
//        {
//            try
//            {
//                var users = await _context
//                    .Users
//                    .Include(u => u.Posts)
//                    .Include(u => u.Comments)
//                    .Include(u => u.Likes)
//                    .Include(u => u.Followers)
//                    .Include(u => u.Followings)
//                    .Include(u => u.UserRoles)
//                    .ThenInclude(ur => ur.Role)
//                    .ToListAsync();

//                var userDtos = users.Select(u => new GetUserDto()
//                {
//                    IdUser = u.Id,
//                    Name = u.Name,
//                    Surname = u.Surname,
//                    Nickname = u.Nickname,
//                    ProfileDescription = u.ProfileDescription,
//                    ProfilePicture = u.ProfilePicture,
//                    DateOfBirth = u.DateOfBirth,
//                    DateOfRegistration = u.DateOfRegistration,
//                    Posts = u.Posts.Select(p => new PostSimpleDto
//                    {
//                        IdPost = p.IdPost,
//                        PostImage = p.PostImage,
//                        PostDate = p.PostDate,
//                        Description = p.Description,
//                        PostCategories = p.PostCategories.Select(pc => new PostCategorySimpleDto
//                        {
//                            CategoryId = pc.CategoryId,
//                            PostId = pc.PostId,
//                            Category = new List<GetCategoriesDto>
//                            {
//                                new GetCategoriesDto
//                                {
//                                    IdCategory = pc.Category.IdCategory,
//                                    CategoryName = pc.Category.CategoryName
//                                }
//                            }
//                        }).ToList(),
//                    }).ToList(),
//                    Comments = u.Comments.Select(c => new CommentSimpleDto
//                    {
//                        IdComment = c.IdComment,
//                        CommentText = c.CommentText,
//                        CommentDate = c.CommentDate,
//                        IdPost = c.IdPost
//                    }).ToList(),
//                    Likes = u.Likes.Select(l => new LikeSimpleDto
//                    {
//                        IdLike = l.IdLike,
//                        IdPost = l.IdPost,
//                        LikeDate = l.LikeDate,
//                    }).ToList(),
//                    Followings = u.Followings.Select(f => new FollowSimpleDto
//                    {
//                        IdFollowed = f.IdFollowed,
//                        IdFollower = f.IdFollower
//                    }).ToList(),
//                    Followers = u.Followers.Select(f => new FollowSimpleDto
//                    {
//                        IdFollowed = f.IdFollowed,
//                        IdFollower = f.IdFollower
//                    }).ToList()
//                }).ToList();

//                return userDtos;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error retrieving users: {ex.Message}");
//                return null;
//            }
//        }

//        public async Task<ApplicationUser> GetUserByNicknameAsync(string nickname)
//        {
//            return await _userManager.Users.OfType<ApplicationUser>().FirstOrDefaultAsync(u => u.Nickname == nickname);
//        }

//        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//            {
//                return false;
//            }

//            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
//            return result.Succeeded;
//        }
//        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
//        {
//            var user = await _userManager.FindByEmailAsync(email);
//            if (user == null)
//            {
//                return false;
//            }

//            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
//            return result.Succeeded;
//        }

//        public async Task<IdentityResult> EditUserAsync(ApplicationUser appUser)
//        {
//            try
//            {
//                var user = await _userManager.FindByIdAsync(appUser.Id);
//                if (user == null)
//                {
//                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });
//                }
//                appUser.Nickname = appUser.Nickname;
//                appUser.ProfilePicture = appUser.ProfilePicture;
//                appUser.PhoneNumber = appUser.PhoneNumber;
//                appUser.ProfileDescription = appUser.ProfileDescription;
//                var result = await _userManager.UpdateAsync(user);
//                if (result.Succeeded)
//                {
//                    _logger.LogInformation($"User {user.UserName} updated successfully.");
//                }
//                else
//                {
//                    _logger.LogError($"Error updating user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//                }
//                return result;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error updating user: {ex.Message}");
//                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
//            }
//        }
//    }
//}
