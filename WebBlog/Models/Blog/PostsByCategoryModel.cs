using WebBlog.Data.Entity.Blog;

namespace WebBlog.Models.Blog
{
    public class PostsByCategoryModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public Category Category { get; set; } = default!;
    }
}
