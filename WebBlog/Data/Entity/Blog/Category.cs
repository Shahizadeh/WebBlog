using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlog.Data.Entity.Blog
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public string Name { get; set; } = default!;

        public virtual ICollection<PostCategory> PostCategories { get; set; } = default!;
    }
}
