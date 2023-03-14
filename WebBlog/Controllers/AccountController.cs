using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.Authorization;
using WebBlog.Services.Authorization;

namespace WebBlog.Controllers
{
    public class AccountController : Controller
    {
        private UserService UserService { get; }
        public AccountController(UserService userService)
        {
            UserService = userService;
        }
        public IActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await UserService.RegisterUser(model);
                if (result.Success)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            return View(model);
        }
    }
}
