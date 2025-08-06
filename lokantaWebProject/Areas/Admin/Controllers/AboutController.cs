using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FindAsync, FirstOrDefaultAsync için

namespace lokantaWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class AboutController : Controller
    {
        private readonly AdminDbContext _context; // Veritabanı bağlamı

        public AboutController(AdminDbContext context)
        {
            _context = context; // Dependency Injection ile bağlamı başlat
        }

        // GET: /About/
        // Tüm hakkımızda bilgilerini listeleyen ana sayfa
        public async Task<IActionResult> Index()
        {
            // Tüm hakkımızda bilgilerini veritabanından asenkron olarak çek ve View'e gönder
            var abouts = await _context.Abouts.ToListAsync();
            return View(abouts);
        }

        // GET: /About/Details/5
        // Belirli bir hakkımızda bilgisinin detaylarını gösterir
        public async Task<IActionResult> Details(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            // Bilgiyi View'e gönder
            return View(about);
        }

        // GET: /About/Create
        // Yeni hakkımızda bilgisi oluşturma formunu gösterir
        public IActionResult Create()
        {
            return View();
        }

        // POST: /About/Create
        // Yeni hakkımızda bilgisini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(About about)
        {
            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                _context.Add(about); // Bilgiyi bağlama ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
            }
            // Doğrulama başarısız ise aynı View'i hatalarla birlikte geri döndür
            return View(about);
        }

        // GET: /About/Edit/5
        // Belirli bir hakkımızda bilgisini düzenleme formunu gösterir
        public async Task<IActionResult> Edit(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FindAsync(id); // Bilgiyi Id'ye göre bul
            if (about == null)
            {
                return NotFound();
            }
            // Bilgiyi View'e gönder
            return View(about);
        }

        // POST: /About/Edit/5
        // Düzenlenmiş hakkımızda bilgisini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Edit(int id, About about)
        {
            // URL'deki Id ile formdan gelen Id uyuşmuyorsa hata döndür
            if (id != about.Id)
            {
                return NotFound();
            }

            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(about); // Bilgiyi güncellemek için işaretle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Bilgi veritabanında yoksa 404 döndür
                    if (!AboutExists(about.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Diğer konkürans hatalarını fırlat
                    }
                }
                return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
            }
            // Doğrulama başarısız ise aynı View'i hatalarla birlikte geri döndür
            return View(about);
        }

        // GET: /About/Delete/5
        // Belirli bir hakkımızda bilgisini silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            // Bilgiyi View'e gönder
            return View(about);
        }

        // POST: /About/Delete/5
        // Hakkımızda bilgisini veritabanından siler
        [HttpPost, ActionName("Delete")] // POST isteği için "Delete" aksiyon adını kullan
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var about = await _context.Abouts.FindAsync(id); // Bilgiyi Id'ye göre bul
            if (about != null)
            {
                _context.Abouts.Remove(about); // Bilgiyi silmek için işaretle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
        }

        // Hakkımızda bilgisinin veritabanında var olup olmadığını kontrol eden yardımcı metot
        private bool AboutExists(int id)
        {
            return _context.Abouts.Any(e => e.Id == id);
        }
    }
}