using Microsoft.AspNetCore.Identity;
using WebBlog.Data.Entity.Authorizatiton;
using WebBlog.Data.Entity.Blog;

namespace WebBlog.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var Context = serviceProvider.GetRequiredService<AppDbContext>();
            using var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            using var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            if((await RoleManager.FindByNameAsync("User")) == null)
                await RoleManager.CreateAsync(new IdentityRole<int> { Name = "User" });
            if ((await RoleManager.FindByNameAsync("Admin")) == null)
                await RoleManager.CreateAsync(new IdentityRole<int> { Name = "Admin" });

            if((await UserManager.FindByNameAsync("admin@mail.com")) == null)
            {
                var admin = new AppUser
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    FirstName = "Admin",
                    LastName = "Admin"
                };
                await UserManager.CreateAsync(admin, "123456");
                await UserManager.AddToRoleAsync(admin, "Admin");
            }

            if(!Context.Categories.Any())
            {
                var categories = new Category[] {
                    new Category
                    {
                        Id= 1,
                        Name= "Cat-1"
                    },
                    new Category
                    {
                        Id= 2,
                        Name= "Cat-2"
                    },
                    new Category
                    {
                        Id= 3,
                        Name= "Cat-3"
                    },
                    new Category
                    {
                        Id= 4,
                        Name= "Cat-4"
                    },
                    new Category
                    {
                        Id= 5,
                        Name= "Cat-5"
                    }
                };
                await Context.Categories.AddRangeAsync(categories);
                await Context.SaveChangesAsync();
            }
        }
    }
}
