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

// ASP.NET Core Identity Yapılandırması (GÜVENLİK İYİLEŞTİRMELERİ BURADA!)
builder.Services.AddIdentity<Admin, IdentityRole<int>>(options =>
{
    // *** Hesap Onayını Zorunlu Yapın ***
    // Adminler için bu MUTLAKA TRUE olmalı. E-posta onaylanmadan hesaplar aktif olmaz.
    options.SignIn.RequireConfirmedAccount = true;

    // *** Güçlü Şifre Politikası Uygulayın ***
    // Bu ayarlar, admin panelinizin brute-force saldırılarına karşı direncini artırır.
    options.Password.RequireDigit = true;           // Şifrede en az bir rakam olmalı
    options.Password.RequireLowercase = true;       // Şifrede en az bir küçük harf olmalı
    options.Password.RequireNonAlphanumeric = true; // Şifrede en az bir özel karakter olmalı
    options.Password.RequireUppercase = true;       // Şifrede en az bir büyük harf olmalı
    options.Password.RequiredLength = 10;           // Şifre minimum 10 karakter uzunluğunda olmalı (önceden 4'tü, bu çok zayıftı!)
    options.Password.RequiredUniqueChars = 1;       // Tekrarlayan karakterlere izin verilir, ancak şifrenin genel gücü diğer kurallarla artırıldı

    // *** Hesap Kilitleme Politikasını Tanımlayın ***
    // Başarısız giriş denemelerinden sonra hesapların geçici olarak kilitlenmesini sağlar.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Hesap 5 dakika kilitli kalır
    options.Lockout.MaxFailedAccessAttempts = 5; // 5 başarısız deneme sonrası hesap kilitlenir
    options.Lockout.AllowedForNewUsers = true; // Yeni oluşturulan kullanıcılar için de kilitlemeyi etkinleştir
})
.AddEntityFrameworkStores<AdminDbContext>()
.AddDefaultTokenProviders(); // Şifre sıfırlama, e-posta onayı gibi token'lar için gerekli

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
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
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
        context.Response.Redirect("/Home/Index"); // kullanıcı zaten giriş yaptıysa admin paneline
    }
    await Task.CompletedTask;
});


app.MapRazorPages(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"); 

app.Run();
