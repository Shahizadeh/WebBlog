using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Entity.Authorizatiton;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Data
{
    public class AppDbContext: IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public virtual DbSet<Category> Categories { get; set; } = default!;
        public virtual DbSet<Post> Posts { get; set; } = default!;
        public virtual DbSet<Comment> Comments { get; set; } = default!;
        public virtual DbSet<PostCategory> PostCategories { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PostCategory>()
                .HasKey(e => new
                {
                    e.PostId,
                    e.CategoryId
                });

            builder.Entity<Comment>(action =>
            {
                action.HasOne(e => e.Post)
                    .WithMany(e => e.Comments)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
