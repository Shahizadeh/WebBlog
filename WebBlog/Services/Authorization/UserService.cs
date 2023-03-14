using Microsoft.AspNetCore.Identity;
using WebBlog.Data.Entity.Authorizatiton;
using WebBlog.Models;
using WebBlog.Models.Authorization;

namespace WebBlog.Services.Authorization
{
    public class UserService
    {
        private UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        { 
            _userManager= userManager;
        }

        public async Task<OperationResult> RegisterUser(RegisterModel request)
        {
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                return new OperationResult { Success = true };
            }
            else
            {
                var resp = new OperationResult { Success = false };
                result.Errors.ToList().ForEach(error =>
                {
                    resp.Message += error.Description + "\n";
                });
                return resp;
            }
        }
    }
}
