using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Post;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class LikeService
    {

        private readonly ApplicationDbContext _context;

        public LikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TryLikeSaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<GetLikeDto>> GetLikes()
        {
            try
            {
                var likeList = await _context
                    .Likes.Include(l => l.User)
                    .Include(l => l.Post)
                    .ToListAsync();
                var likeDtoList = likeList
                    .Select(l => new GetLikeDto()
                    {
                        LikeCount = l.LikeCount,
                        LikeDate = l.LikeDate,
                        User = new UserPostDto()
                        {
                            Nickname = l.User.Nickname,
                            ProfilePicture = l.User.ProfilePicture
                        },
                    }).ToList();
                return likeDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving likes.", ex);
            }
        }

        public async Task<bool> CreateLike(CreateLikeDto createLike)
        {
            try
            {
                var like = new Like
                {
                    IdLike = Guid.NewGuid(),
                    LikeDate = DateTime.UtcNow,
                    IsLike = true,
                    LikeCount = 1,
                };
                await _context.Likes.AddAsync(like);
                return await TryLikeSaveAsync();
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An error occurred while creating the like.", ex);
            }
        }

        public async Task<bool> DeleteLike(Guid id)
        {
            try
            {
                var like = await _context.Likes.FindAsync(id);
                if (like == null)
                {
                    return false;
                }
                _context.Likes.Remove(like);
                return await TryLikeSaveAsync();
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An error occurred while deleting the like.", ex);
            }
        }
    }
}
