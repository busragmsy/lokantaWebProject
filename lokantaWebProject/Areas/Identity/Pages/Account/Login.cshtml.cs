using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using lokantaWebProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace lokantaWebProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Entities.Admin> _signInManager;
        private readonly UserManager<Entities.Admin> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly AdminDbContext _context;

        public LoginModel(SignInManager<Entities.Admin> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<Entities.Admin> userManager,
            AdminDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-posta adresi boş bırakılamaz.")] // Hata mesajı eklendi
            [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")] // Hata mesajı eklendi
            public string Email { get; set; }

            [Required(ErrorMessage = "Şifre boş bırakılamaz.")] // Hata mesajı eklendi
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Beni Hatırla")] // Türkçe isim
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    _context.AdminLoginLogs.Add(new AdminLoginLog
                    {
                        UserName = user?.UserName ?? Input.Email,
                        IsSuccessful = true,
                        LoginTime = DateTime.Now,
                        IpAddress = ipAddress,
                        UserAgent = userAgent
                    });
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                if (result.IsLockedOut)
                {
                    // Gerekirse burada da log kaydı yapabilirsin
                    return RedirectToPage("./Lockout");
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                // Başarısız giriş buraya düşer
                _context.AdminLoginLogs.Add(new AdminLoginLog
                {
                    UserName = Input.Email,
                    IsSuccessful = false,
                    LoginTime = DateTime.Now,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                });
                await _context.SaveChangesAsync();

                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                _logger.LogWarning($"Kullanıcı '{Input.Email}' için başarısız giriş denemesi. IP: {ipAddress}");
                return Page();
            }

            return Page(); // ModelState geçersizse form tekrar gösterilir
        }
    }
}
