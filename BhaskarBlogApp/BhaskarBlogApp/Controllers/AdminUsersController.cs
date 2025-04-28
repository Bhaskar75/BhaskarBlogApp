using BhaskarBlogApp.Models.ViewModels;
using BhaskarBlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BhaskarBlogApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUserRepository userRepository,UserManager<IdentityUser> userManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await userRepository.GetAll();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();
            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }
            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request)
        {
            var identyUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var identityResult = await userManager.CreateAsync(identyUser, request.Password);

            if (identityResult is not null)
            {
                if(identityResult.Succeeded)
                {
                    //assign roles to this user
                    var roles = new List<string> { "User" };
                    if(request.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identyUser,roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var iR= await userManager.DeleteAsync(user);
                if (iR != null && iR.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
            return View();
        }
    }
}
