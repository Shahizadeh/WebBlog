using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBlog.Models;
using WebBlog.Models.Blog;
using WebBlog.Services.Blog;

namespace WebBlog.Controllers
{
    public class HomeController : Controller
    {
        private IPostService PostService { get; } 
        private ICategoryService CategoryService { get; }

        public HomeController(IPostService postService, ICategoryService categoryService)
        {
            PostService = postService;
            CategoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageModel();
            model.Posts = await PostService.GetRecentPosts(10);
            model.Categories = await CategoryService.GetAllCategories();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}