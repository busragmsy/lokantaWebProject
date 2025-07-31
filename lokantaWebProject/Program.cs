using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; // IPostConfigureOptions için
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Yapılandırması
builder.Services.AddDbContext<AdminDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity Yapılandırması 
builder.Services.AddIdentity<Admin, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AdminDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();
builder.Services.AddScoped<UserManager<Admin>>();
builder.Services.AddScoped<SignInManager<Admin>>();

// Çerez Kimlik Doğrulama Ayarları
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Oturum 30 dakika sonra sona erer
    options.LoginPath = "/Identity/Account/Login"; // Giriş sayfanızın yolu
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Erişim engellendiğinde yönlendirilecek yol
    options.SlidingExpiration = false; // Admin paneli için false daha güvenlidir. Kullanıcı aktif olsa bile oturum süresi dolunca tekrar giriş yapması gerekir, bu oturum çalınması riskini azaltır.
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Yetkilendirme Politikalarını Tanımlayın
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "SuperAdmin"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
});


var app = builder.Build();

// HTTP İstek İşlem Hattını yapılandırma
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Üretim ortamında detaylı hataları gizler
    app.UseHsts(); // HTTPS kullanıyorsanız HSTS önemlidir, tarayıcılara sadece HTTPS kullanmalarını söyler
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye yönlendirir
app.UseStaticFiles(); // Statik dosyaları (CSS, JS, resimler) sunar

app.UseRouting(); // Yönlendirme middleware'ini etkinleştirir
app.UseAuthentication();
app.UseAuthorization();

// *** HTTP Güvenlik Başlıkları Ekleyin (Çok Önemli!) ***
// Bu başlıklar, tarayıcı tabanlı saldırılara (XSS, Clickjacking) karşı ek bir savunma katmanı sağlar.
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // MIME türü koklamasını engelle
    context.Response.Headers.Add("X-Frame-Options", "DENY"); // Sayfanın iframe içinde gösterilmesini engelle (Clickjacking koruması)
    context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade"); // Referrer bilgisini kısıtlar
    // Content-Security-Policy (CSP) daha karmaşıktır ve uygulamanızın ihtiyaçlarına göre dikkatlice ayarlanmalıdır.
    // Şimdilik eklemiyoruz ancak ileride düşünmelisiniz. Örneğin:
    // context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self'; connect-src 'self'; frame-ancestors 'none';");
    await next();
});

app.MapGet("/", async context =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Identity/Account/Login");
    }
    else
    {
        context.Response.Redirect("/Admin/Home/Index"); 
    }
    await Task.CompletedTask;
});


app.MapRazorPages();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}"); 

app.Run();
