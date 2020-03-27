using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectManager.Web.Models;

namespace ProjectManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] ViewModels.UserLoginViewModel loginViewModel)
        {
            //loginViewModel = new ViewModels.UserLoginViewModel { Email = "cescosoft@gmail.com", Password = "Felice1_" };
            //loginViewModel.Password = "Felice1_";
            if (!ModelState.IsValid) return View(loginViewModel);
            ApplicationUser user = null;

            user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "The User is Blocked!");
                    return View(loginViewModel);
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    SessionHelper.SetUserLoggin(user);
                    if (user.Role == "USER")
                    {
                        return RedirectToAction("Projects", "Users", new { userId = user.Id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid Username or Password");
            return View(loginViewModel);
        }

        // GET: Home/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            SessionHelper.ResetUserLoggin();
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login), "Home");
        }
    }
}
