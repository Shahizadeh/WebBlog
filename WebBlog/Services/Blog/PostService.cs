using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Data.Entity.Blog;
using WebBlog.Models;
using WebBlog.Models.Blog;
using WebBlog.Services.Authorization;

namespace WebBlog.Services.Blog
{
    public interface IPostService
    {
        Task<Post> GetPostById(long postId);
        Task<List<Post>> GetPostsByCategoryId(int categoryId);
        Task<List<Post>> GetRecentPosts(int count);
        Task<OperationResult> SavePost(AddPostModel request, IFormFile photo);
    }
    public class PostService : IPostService
    {
        private AppDbContext DbContext { get; }
        private IWebHostEnvironment Environment { get; }
        private IUserService UserService { get; }
        public PostService(AppDbContext dbContext, IWebHostEnvironment environment, IUserService userService) 
        { 
            DbContext = dbContext; 
            Environment = environment;
            UserService = userService;
        }

        public async Task<OperationResult> SavePost(AddPostModel request, IFormFile photo)
        {
            var User = await UserService.GetAppUser(null);
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                PhotoPath = await UploadPostPhoto(photo),
                UserId = User.Id,
                CreatedOn = DateTimeOffset.Now
            };
            DbContext.Posts.Add(post);
            foreach (var item in request.PostCategories)
            {
                DbContext.PostCategories.Add(new PostCategory
                {
                    Post = post,
                    CategoryId = item
                });
            }
            try
            {
                await DbContext.SaveChangesAsync();
                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private async Task<string> UploadPostPhoto(IFormFile photo)
        {
            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            using(FileStream file = new FileStream(Environment.WebRootPath + "/Uploads/Posts/" + FileName, FileMode.Create))
            {
                await photo.CopyToAsync(file);
            }
            return FileName;
        }

        public async Task<List<Post>> GetRecentPosts(int count)
        {
            var query = DbContext.Posts.AsQueryable();

            query = query.OrderByDescending(e => e.CreatedOn);

            return await query.Take(count).ToListAsync();

        }

        public async Task<Post> GetPostById(long postId)
        {
            var query = DbContext.Posts.AsQueryable();

            query = query
                .Include(e => e.PostCategories)
                .ThenInclude(e => e.Category)
                .Include(e => e.Comments)
                .ThenInclude(e => e.User)
                .Include(e => e.User);

            query = query.Where(e => e.Id == postId);

            return await query.SingleAsync();
        }

        public async Task<List<Post>> GetPostsByCategoryId(int categoryId)
        {
            var query = DbContext.Posts.AsQueryable();

            query = from q in query
                    join d in DbContext.PostCategories on q.Id equals d.PostId
                    where d.CategoryId == categoryId
                    select q;

            query = query.OrderByDescending(e => e.CreatedOn);

            return await query.Take(10).ToListAsync();
        }
    }
}
