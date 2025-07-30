using lokantaWebProject.Entities;
using lokantaWebProject.Models;
using lokantaWebProject.Models;
using lokantaWebProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace lokantaWebProject.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SettingsController : Controller
    {
        private readonly UserManager<Admin> _userManager;
        private readonly SignInManager<Admin> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public SettingsController(UserManager<Admin> userManager,
                                  SignInManager<Admin> signInManager,
                                  RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Şifre Değiştir GET
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // DEBUG: buraya log veya breakpoint koyabilirsin
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("ChangePassword");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        // Yeni Admin Oluştur GET
        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // Yeni Admin Oluştur POST
        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newAdmin = new Admin
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                CreatedDate = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newAdmin, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newAdmin, model.Role);
                TempData["Success"] = "Yeni admin başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
