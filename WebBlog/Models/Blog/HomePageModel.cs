using WebBlog.Data.Entity.Blog;

namespace WebBlog.Models.Blog
{
    public class HomePageModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
