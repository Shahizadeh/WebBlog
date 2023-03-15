using System.ComponentModel.DataAnnotations;

namespace WebBlog.Data.Entity.Blog
{
    public class PostCategory
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = default!;
        public long PostId { get; set; }
        public virtual Post Post { get; set; } = default!;
    }
}
