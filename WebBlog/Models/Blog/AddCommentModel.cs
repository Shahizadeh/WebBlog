namespace WebBlog.Models.Blog
{
    public class AddCommentModel
    {
        public long PostId { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
