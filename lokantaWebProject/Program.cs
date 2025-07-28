using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; // IPostConfigureOptions için gerekli
using System; 
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// DbContext'i kaydetme
builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity servislerini özel Admin sınıfınız ve IdentityRole<int> ile ekleyin
// **DİKKAT: .AddApplicationCookie() veya .AddCookie() zincirini buradan kaldırdık.**
builder.Services.AddIdentity<Admin, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<AdminDbContext>()
.AddDefaultTokenProviders();

// Identity'nin varsayılan uygulama çerezi ayarlarını özelleştirmek için PostConfigure kullanın
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Identity/Account/Login"; // Giriş sayfanızın yolu
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


// IEmailSender servisini ekleyin
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Controller'ları ve View'ları ekle
builder.Services.AddControllersWithViews();

// RAZOR PAGES SERVİSİNİ EKLEYİN
builder.Services.AddRazorPages();

var app = builder.Build();

// HTTP İstek İşlem Hattını yapılandırma
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik Doğrulama ve Yetkilendirme middleware'lerini ekleyin
app.UseAuthentication();
app.UseAuthorization();

// Identity'nin Razor Pages arayüzünü haritalamak için
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Basit (dummy) bir IEmailSender implementasyonu
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        Console.WriteLine($"To: {email}, Subject: {subject}");
        Console.WriteLine($"Message: {htmlMessage}");
        return Task.CompletedTask;
    }
}