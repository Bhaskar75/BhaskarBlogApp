using BhaskarBlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BhaskarBlogApp.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> userManager { get; }

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
            };
            var idenetityResult = await userManager.CreateAsync(identityUser,registerViewModel.Password);

            if (idenetityResult.Succeeded)
            {
                //assign this user the "user" role
                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser,"User");
                if (roleIdentityResult.Succeeded)
                {
                    //Show success notification
                    return RedirectToAction("Register");
                }
            }

            return View();
        }
    }
}
