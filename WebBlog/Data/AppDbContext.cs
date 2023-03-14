using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Entity.Authorizatiton;

namespace WebBlog.Data
{
    public class AppDbContext: IdentityDbContext<AppUser,IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

    }
}
