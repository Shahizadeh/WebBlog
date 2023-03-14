using Microsoft.AspNetCore.Identity;
using WebBlog.Data.Entity.Authorizatiton;

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

        }
    }
}
