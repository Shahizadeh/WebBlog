using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Data.Entity.Blog;
using WebBlog.Models.Blog;
using WebBlog.Services.Blog;

namespace WebBlog.Controllers
{
    [Route("{controller}/{action}")]
    public class PostController : Controller
    {
        private ICategoryService CategoryService { get; }
        private IPostService PostService { get; }
        public PostController(ICategoryService categoryService, IPostService postService) 
        { 
            CategoryService = categoryService;
            PostService = postService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddPostModel();
            model.Categories = await CategoryService.GetAllCategories();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddPostModel model)
        {
            var photo = Request.Form.Files.GetFile("PhotoPath");
            if (ModelState.IsValid)
            {
                if(photo != null && photo.ContentType.StartsWith("image/"))
                {
                    var result = await PostService.SavePost(model, photo);
                    if (result.Success)
                    {
                        TempData.Add("Message", "Post Saved Successfully!");
                        return RedirectToAction("Add");
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Image File!");
                }

            }
            model.Categories = await CategoryService.GetAllCategories();
            return View(model);
        }


        [HttpGet("{postId}")]
        public async Task<IActionResult> Detail(long postId)
        {
            var post = await PostService.GetPostById(postId);
            return View(post);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var model = new PostsByCategoryModel();
            model.Posts = await PostService.GetPostsByCategoryId(categoryId);
            model.Category = await CategoryService.GetById(categoryId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            var model = new ExportPostsModel();
            model.StartDate = DateTime.Now;
            model.Categories = await CategoryService.GetAllCategories();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Export(ExportPostsModel request)
        {
            var result = await PostService.ExportPosts(request);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchPostsModel request)
        {
            var posts = await PostService.SearchByTitle(request);
            request.Posts = posts;
            return View(request);
        }
    }
}
