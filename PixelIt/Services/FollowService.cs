using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Follow;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class FollowService
    {
        private readonly ApplicationDbContext _context;

        public FollowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TryFollowSaveAsync()
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

        public async Task<List<GetFollowDto>> GetFollows()
        {
            try
            {
                var followList = await _context
                    .Follows.Include(f => f.Follower)
                    .Include(f => f.Followed)
                    .ToListAsync();
                var followDtoList = followList
                    .Select(f => new GetFollowDto()
                    {
                        Follower = new UserSimpleDto()
                        {
                            Nickname = f.Follower.Nickname,
                            ProfilePicture = f.Follower.ProfilePicture
                        },
                        Followed = new UserSimpleDto()
                        {
                            Nickname = f.Followed.Nickname,
                            ProfilePicture = f.Followed.ProfilePicture
                        }
                    }).ToList();
                return followDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving follows.", ex);
            }

        }

        public async Task<bool> CreateFollow(CreateFollowDto createFollow)
        {
            try
            {
                var follow = new Follow
                {
                    IdFollow = Guid.NewGuid(),
                    DateFollow = DateTime.UtcNow
                };
                _context.Follows.Add(follow);
                return await TryFollowSaveAsync();
            }
            catch (Exception ex)
            {
                return false;

                throw new Exception("An error occurred while creating the follow.", ex);
            }
        }

        public async Task<bool> DeleteFollow(Guid id)
        {
            try
            {
                var follow = await _context.Follows.FindAsync(id);
                if (follow == null)
                {
                    return false;
                }
                _context.Follows.Remove(follow);
                return await TryFollowSaveAsync();
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An error occurred while deleting the follow.", ex);
            }
        }
    }
}
