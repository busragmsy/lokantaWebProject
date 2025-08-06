using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FindAsync, FirstOrDefaultAsync için

namespace lokantaWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class GalleryImageController : Controller
    {
        private readonly AdminDbContext _context; // Veritabanı bağlamı

        public GalleryImageController(AdminDbContext context)
        {
            _context = context; // Dependency Injection ile bağlamı başlat
        }

        // GET: /GalleryImage/
        // Tüm galeri görsellerini listeleyen ana sayfa
        public async Task<IActionResult> Index()
        {
            // Tüm galeri görsellerini veritabanından asenkron olarak çek ve View'e gönder
            var galleryImages = await _context.GalleryImages.ToListAsync();
            return View(galleryImages);
        }

        // GET: /GalleryImage/Details/5
        // Belirli bir galeri görselinin detaylarını gösterir
        public async Task<IActionResult> Details(int? id)
        {
            // Id null ise veya görsel bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryImage == null)
            {
                return NotFound();
            }

            // Görseli View'e gönder
            return View(galleryImage);
        }

        // GET: /GalleryImage/Create
        // Yeni galeri görseli oluşturma formunu gösterir
        public IActionResult Create()
        {
            return View();
        }

        // POST: /GalleryImage/Create
        // Yeni galeri görselini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(GalleryImage galleryImage)
        {
            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                _context.Add(galleryImage); // Görseli bağlama ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
            }
            // Doğrulama başarısız ise aynı View'i hatalarla birlikte geri döndür
            return View(galleryImage);
        }

        // GET: /GalleryImage/Edit/5
        // Belirli bir galeri görselini düzenleme formunu gösterir
        public async Task<IActionResult> Edit(int? id)
        {
            // Id null ise veya görsel bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImages.FindAsync(id); // Görseli Id'ye göre bul
            if (galleryImage == null)
            {
                return NotFound();
            }
            // Görseli View'e gönder
            return View(galleryImage);
        }

        // POST: /GalleryImage/Edit/5
        // Düzenlenmiş galeri görselini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Edit(int id, GalleryImage galleryImage)
        {
            // URL'deki Id ile formdan gelen Id uyuşmuyorsa hata döndür
            if (id != galleryImage.Id)
            {
                return NotFound();
            }

            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(galleryImage); // Görseli güncellemek için işaretle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Görsel veritabanında yoksa 404 döndür
                    if (!GalleryImageExists(galleryImage.Id))
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
            return View(galleryImage);
        }

        // GET: /GalleryImage/Delete/5
        // Belirli bir galeri görselini silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            // Id null ise veya görsel bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var galleryImage = await _context.GalleryImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (galleryImage == null)
            {
                return NotFound();
            }

            // Görseli View'e gönder
            return View(galleryImage);
        }

        // POST: /GalleryImage/Delete/5
        // Galeri görselini veritabanından siler
        [HttpPost, ActionName("Delete")] // POST isteği için "Delete" aksiyon adını kullan
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var galleryImage = await _context.GalleryImages.FindAsync(id); // Görseli Id'ye göre bul
            if (galleryImage != null)
            {
                _context.GalleryImages.Remove(galleryImage); // Görseli silmek için işaretle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
        }

        // Galeri görselinin veritabanında var olup olmadığını kontrol eden yardımcı metot
        private bool GalleryImageExists(int id)
        {
            return _context.GalleryImages.Any(e => e.Id == id);
        }
    }
}