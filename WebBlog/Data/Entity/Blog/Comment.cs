using System.ComponentModel.DataAnnotations.Schema;
using WebBlog.Data.Entity.Authorizatiton;

namespace WebBlog.Data.Entity.Blog
{
    public class Comment
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public virtual Post Post { get; set; } = default!;
        [Column(TypeName = "nvarchar(256)")]
        public string Content { get; set; } = default!;
        public int UserId { get; set; }
        public virtual AppUser User { get; set; } = default!;
        public DateTimeOffset CreatedOn { get; set; }

    }
}
