using System.ComponentModel.DataAnnotations;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Models.Blog
{
    public class AddPostModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = default!;

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; } = default!;

        [Display(Name = "Post Photo")]
        public string? PhotoPath { get; set; } = default!;

        [Required]
        [Display(Name ="Categories")]
        public List<int> PostCategories { get; set; } = default!;

        public List<Category> Categories { get; set; } = new List<Category>();

    }
}
