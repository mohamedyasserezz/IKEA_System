using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.PL.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinkDev.IKEA.PL.Controllers
{
    public class AccountController(
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager) : Controller
    {
        #region Sign UP
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    FName = model.FirstName,
                    LName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                    RedirectToAction(nameof(SignIn));

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
                ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This User Name is already exist");

            return View(model);
        }
        #endregion

        #region Sign In
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user is { }) 
            {
                var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "Your Account is not allowed!!");
                    if (result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your Account is Locked!!");
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
               
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View(model);
        }
        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion
    }
}
