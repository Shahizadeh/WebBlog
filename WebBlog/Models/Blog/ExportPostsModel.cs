using System.ComponentModel.DataAnnotations;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Models.Blog
{
    public class ExportPostsModel
    {
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public List<Category> Categories { get; set; } = default!;
    }
}
