using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Post;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> TryCommentSaveAsync()
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

        public async Task<List<GetCommentDto>> GetComments()
        {
            try
            {
                var commentList = await _context
                    .Comments.Include(c => c.User)
                    .Include(c => c.Post)
                    .ToListAsync();
                var commentDtoList = commentList
                    .Select(c => new GetCommentDto()
                    {
                        CommentText = c.CommentText,
                        CommentDate = c.CommentDate,
                        User = new UserPostDto()
                        {
                            Nickname = c.User.Nickname,
                            ProfilePicture = c.User.ProfilePicture
                        },
                        Post = new PostCommentDto()
                        {
                            IdPost = c.Post.IdPost,
                        }
                    }).ToList();
                return commentDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving comments: " + ex.Message);
            }
        }

        public async Task<bool> CreateComment(CreateCommentDto commentDto)
        {
            try
            {
                var comment = new Comment
                {
                    CommentText = commentDto.CommentText,
                    CommentDate = DateTime.UtcNow,
                };
                await _context.Comments.AddAsync(comment);
                return await TryCommentSaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating comment: " + ex.Message);
            }
        }

        public async Task<bool> EditComment(Guid id, EditCommentDto commentDto)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                {
                    return false;
                }
                comment.CommentText = commentDto.CommentText;
                comment.CommentDate = DateTime.UtcNow;
                _context.Comments.Update(comment);
                return await TryCommentSaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating comment: " + ex.Message);
            }
        }

        public async Task<bool> DeleteComment(Guid id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                {
                    return false;
                }
                _context.Comments.Remove(comment);
                return await TryCommentSaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting comment: " + ex.Message);
            }
        }
    }
}

