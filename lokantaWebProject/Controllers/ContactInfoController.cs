using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FindAsync, FirstOrDefaultAsync için

namespace lokantaWebProject.Controllers
{
    public class ContactInfoController : Controller
    {
        private readonly AdminDbContext _context; // Veritabanı bağlamı

        public ContactInfoController(AdminDbContext context)
        {
            _context = context; // Dependency Injection ile bağlamı başlat
        }

        // GET: /ContactInfo/
        // Tüm iletişim bilgilerini listeleyen ana sayfa
        public async Task<IActionResult> Index()
        {
            // Tüm iletişim bilgilerini veritabanından asenkron olarak çek ve View'e gönder
            var contactInfos = await _context.ContactInfos.ToListAsync();
            return View(contactInfos);
        }

        // GET: /ContactInfo/Details/5
        // Belirli bir iletişim bilgisinin detaylarını gösterir
        public async Task<IActionResult> Details(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            // Bilgiyi View'e gönder
            return View(contactInfo);
        }

        // GET: /ContactInfo/Create
        // Yeni iletişim bilgisi oluşturma formunu gösterir
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ContactInfo/Create
        // Yeni iletişim bilgisini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(ContactInfo contactInfo)
        {
            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                _context.Add(contactInfo); // Bilgiyi bağlama ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
            }
            // Doğrulama başarısız ise aynı View'i hatalarla birlikte geri döndür
            return View(contactInfo);
        }

        // GET: /ContactInfo/Edit/5
        // Belirli bir iletişim bilgisini düzenleme formunu gösterir
        public async Task<IActionResult> Edit(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos.FindAsync(id); // Bilgiyi Id'ye göre bul
            if (contactInfo == null)
            {
                return NotFound();
            }
            // Bilgiyi View'e gönder
            return View(contactInfo);
        }

        // POST: /ContactInfo/Edit/5
        // Düzenlenmiş iletişim bilgisini veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Edit(int id, ContactInfo contactInfo)
        {
            // URL'deki Id ile formdan gelen Id uyuşmuyorsa hata döndür
            if (id != contactInfo.Id)
            {
                return NotFound();
            }

            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactInfo); // Bilgiyi güncellemek için işaretle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Bilgi veritabanında yoksa 404 döndür
                    if (!ContactInfoExists(contactInfo.Id))
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
            return View(contactInfo);
        }

        // GET: /ContactInfo/Delete/5
        // Belirli bir iletişim bilgisini silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            // Id null ise veya bilgi bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var contactInfo = await _context.ContactInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            // Bilgiyi View'e gönder
            return View(contactInfo);
        }

        // POST: /ContactInfo/Delete/5
        // İletişim bilgisini veritabanından siler
        [HttpPost, ActionName("Delete")] // POST isteği için "Delete" aksiyon adını kullan
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactInfo = await _context.ContactInfos.FindAsync(id); // Bilgiyi Id'ye göre bul
            if (contactInfo != null)
            {
                _context.ContactInfos.Remove(contactInfo); // Bilgiyi silmek için işaretle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
        }

        // İletişim bilgisinin veritabanında var olup olmadığını kontrol eden yardımcı metot
        private bool ContactInfoExists(int id)
        {
            return _context.ContactInfos.Any(e => e.Id == id);
        }
    }
}