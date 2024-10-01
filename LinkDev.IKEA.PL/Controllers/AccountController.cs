using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.PL.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
		#endregion
	}
}
