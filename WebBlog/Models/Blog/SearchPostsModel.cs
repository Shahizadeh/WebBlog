using System.ComponentModel.DataAnnotations;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Models.Blog
{
    public class SearchPostsModel
    {
        [Required]
        [Display(Name = "Post Title")]
        public string Title { get; set; } = default!;
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
