using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalWorkshop.Controllers
{
	public class AccountController : Controller
	{
		protected UserManager<IdentityUser> UserManager { get; }
		protected SignInManager<IdentityUser> SignInManager { get; }
		protected RoleManager<IdentityRole> RoleManager { get; }

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			RoleManager = roleManager;
		}

		public async Task<IActionResult> AddRole()
		{
			var ir = new IdentityRole("Admin");
			await RoleManager.CreateAsync(ir);
			return Content("Dodano Rolę");
		}

		public async Task<IActionResult> AddUserToRole()
		{
			IdentityUser user = await UserManager.GetUserAsync(User);
			await UserManager.AddToRoleAsync(user, "Admin");
			return RedirectToAction("Index", "Main");
		}

		public async Task<IActionResult> GetUser()
		{
			IdentityUser user = await UserManager.GetUserAsync(User);
			return View(user);
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser(viewModel.Login) { Email = viewModel.Email };
				var result = await UserManager.CreateAsync(user, viewModel.Password);

				if (result.Succeeded)
				{
					await SignInManager.PasswordSignInAsync(viewModel.Login,
						viewModel.Password, true, false);
					return RedirectToAction("Index", "Main");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(viewModel);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var result = await SignInManager.PasswordSignInAsync(viewModel.Login,
					viewModel.Password, viewModel.RememberMe, false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Main");
				}
				else
				{
					ModelState.AddModelError("x","Niepoprawny Login lub Hasło");
				}
			}
			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> LogOut()
		{
			await SignInManager.SignOutAsync();
			return RedirectToAction("Index", "Main");
		}
	}
}


