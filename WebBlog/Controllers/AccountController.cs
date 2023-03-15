using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.Authorization;
using WebBlog.Services.Authorization;

namespace WebBlog.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService { get; }
        public AccountController(IUserService userService)
        {
            UserService = userService;
        }
        [HttpGet("{controller}/{action}/{returnUrl?}")]
        public IActionResult Register(string? returnUrl)
        {
            var model = new RegisterModel();
            model.ReturnUrl = returnUrl;
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
        [HttpGet("{controller}/{action}/{returnUrl?}")]
        public IActionResult Login(string? returnUrl)
        {
            var model = new LoginModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await UserService.Login(model);
                if (result.Success)
                {
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid SignIn Try!");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await UserService.SignOut();
            return Redirect("/");
        }
    }
}
