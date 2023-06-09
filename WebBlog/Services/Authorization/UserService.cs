﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebBlog.Data.Entity.Authorizatiton;
using WebBlog.Models;
using WebBlog.Models.Authorization;

namespace WebBlog.Services.Authorization
{
    public interface IUserService
    {
        Task<OperationResult> Login(LoginModel model);
        Task<OperationResult> RegisterUser(RegisterModel request);
        Task SignOut();
        Task<AppUser> GetAppUser(ClaimsPrincipal? user);
    }
    public class UserService: IUserService
    {
        private UserManager<AppUser> UserManager { get; }
        private SignInManager<AppUser> SignInManager { get; }
        private IHttpContextAccessor ContextAccessor { get; }
        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IHttpContextAccessor contextAccessor)
        { 
            UserManager= userManager;
            SignInManager= signInManager;
            ContextAccessor= contextAccessor;
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
            var result = await UserManager.CreateAsync(user, request.Password);

            if(result.Succeeded)
            {
                await UserManager.AddToRoleAsync(user, "User");

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

        public async Task<OperationResult> Login(LoginModel request)
        {
            var signInResult = await SignInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, true);
            if (signInResult.Succeeded)
            {
                return new OperationResult { Success = true };
            }
            else
            {
                return new OperationResult { Success = false };
            }
        }

        public async Task SignOut()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task<AppUser> GetAppUser(ClaimsPrincipal? user)
        {
            user = user ?? ContextAccessor.HttpContext?.User;
            return await UserManager.GetUserAsync(user);
        }
    }
}
