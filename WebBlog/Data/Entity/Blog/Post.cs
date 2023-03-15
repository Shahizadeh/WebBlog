using System.ComponentModel.DataAnnotations.Schema;
using WebBlog.Data.Entity.Authorizatiton;

namespace WebBlog.Data.Entity.Blog
{
    public class Post
    {
        public long Id { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        [Column(TypeName = "nvarchar(1024)")]
        public string PhotoPath { get; set; } = default!;
        public int UserId { get; set; }
        public virtual AppUser User { get; set; } = default!;
        public DateTimeOffset CreatedOn { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = default!;
        public virtual ICollection<PostCategory> PostCategories { get; set; } = default!;
    }
}
