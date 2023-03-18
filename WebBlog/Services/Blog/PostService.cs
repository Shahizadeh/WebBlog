using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebBlog.Data;
using WebBlog.Data.Entity.Blog;
using WebBlog.Models;
using WebBlog.Models.Blog;
using WebBlog.Services.Authorization;

namespace WebBlog.Services.Blog
{
    public interface IPostService
    {
        Task<byte[]> ExportPosts(ExportPostsModel request);
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

        public async Task<byte[]> ExportPosts(ExportPostsModel request)
        {
            var query = DbContext.Posts.AsQueryable();
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(e => e.CreatedOn >= request.StartDate.Value.Date && e.CreatedOn <= request.EndDate.Value.Date);
            }
            else if (request.StartDate.HasValue)
            {
                query = query.Where(e => e.CreatedOn.Date == request.StartDate.Value.Date);
            }

            if (request.CategoryId.HasValue)
            {
                query = from q in query
                        join d in DbContext.PostCategories on q.Id equals d.PostId
                        where d.CategoryId == request.CategoryId.Value
                        select q;
            }

            var posts = await query
                .Include(e => e.User)
                .OrderByDescending(e => e.CreatedOn).ToListAsync();

            using var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);

            var sheet = package.Workbook.Worksheets.Add("Posts");

            //posts with colums: Nr, Id, Title, Description, Full Name of Creator
            int row = 1;
            sheet.Cells[row, 1].Value = "Nr";
            sheet.Cells[row, 2].Value = "Id";
            sheet.Cells[row, 3].Value = "Title";
            sheet.Cells[row, 4].Value = "Description";
            sheet.Cells[row, 5].Value = "Creator";
            row++;

            foreach (var post in posts)
            {
                sheet.Cells[row, 1].Value = row - 1;
                sheet.Cells[row, 2].Value = post.Id;
                sheet.Cells[row, 3].Value = post.Title;
                sheet.Cells[row, 4].Value = post.Content;
                sheet.Cells[row, 5].Value = post.User.FirstName + " " + post.User.LastName;
                row++;
            }

            package.Save();

            return stream.ToArray();
        }
    }
}
