using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Data.Entity.Blog;
using WebBlog.Models;
using WebBlog.Models.Blog;
using WebBlog.Services.Authorization;

namespace WebBlog.Services.Blog
{
    public interface ICommentService
    {
        Task<OperationResult> Add(AddCommentModel request);
        Task<List<Comment>> GetByPostId(long postId);
    }

    public class CommentService : ICommentService
    {
        private AppDbContext DbContext { get; }
        private IUserService UserService { get; }
        public CommentService(AppDbContext dbContext, IUserService userService) 
        { 
            DbContext = dbContext; 
            UserService = userService;
        }
        public async Task<OperationResult> Add(AddCommentModel request)
        {
            var User = await UserService.GetAppUser(null);
            var comment = new Comment
            {
                PostId = request.PostId,
                Content = request.Content,
                UserId = User.Id,
                CreatedOn = DateTimeOffset.Now,
            };
            DbContext.Comments.Add(comment);
            try
            {
                await DbContext.SaveChangesAsync();
                return new OperationResult { Success= true };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<List<Comment>> GetByPostId(long postId)
        {
            return await DbContext.Comments
                .Include(e => e.User)
                .OrderByDescending(e => e.CreatedOn)
                .Where(e => e.PostId== postId)
                .ToListAsync();
        }
    }
}
