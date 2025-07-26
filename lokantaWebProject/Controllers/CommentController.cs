using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FindAsync, FirstOrDefaultAsync için

namespace lokantaWebProject.Controllers
{
    public class CommentController : Controller
    {
        private readonly AdminDbContext _context; // Veritabanı bağlamı

        public CommentController(AdminDbContext context)
        {
            _context = context; // Dependency Injection ile bağlamı başlat
        }

        // GET: /Comment/
        // Tüm yorumları listeleyen ana sayfa
        public async Task<IActionResult> Index()
        {
            // Tüm yorumları veritabanından asenkron olarak çek ve View'e gönder
            var comments = await _context.Comments.ToListAsync();
            return View(comments);
        }

        // GET: /Comment/Details/5
        // Belirli bir yorumun detaylarını gösterir
        public async Task<IActionResult> Details(int? id)
        {
            // Id null ise veya yorum bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            // Yorumu View'e gönder
            return View(comment);
        }

        // GET: /Comment/Create
        // Yeni yorum oluşturma formunu gösterir
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Comment/Create
        // Yeni yorumu veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(Comment comment)
        {
            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                _context.Add(comment); // Yorumu bağlama ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
            }
            // Doğrulama başarısız ise aynı View'i hatalarla birlikte geri döndür
            return View(comment);
        }

        // GET: /Comment/Edit/5
        // Belirli bir yorumu düzenleme formunu gösterir
        public async Task<IActionResult> Edit(int? id)
        {
            // Id null ise veya yorum bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id); // Yorumu Id'ye göre bul
            if (comment == null)
            {
                return NotFound();
            }
            // Yorumu View'e gönder
            return View(comment);
        }

        // POST: /Comment/Edit/5
        // Düzenlenmiş yorumu veritabanına kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            // URL'deki Id ile formdan gelen Id uyuşmuyorsa hata döndür
            if (id != comment.Id)
            {
                return NotFound();
            }

            // Model doğrulaması başarılı ise
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment); // Yorumu güncellemek için işaretle
                    await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Yorum veritabanında yoksa 404 döndür
                    if (!CommentExists(comment.Id))
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
            return View(comment);
        }

        // GET: /Comment/Delete/5
        // Belirli bir yorumu silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            // Id null ise veya yorum bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            // Yorumu View'e gönder
            return View(comment);
        }

        // POST: /Comment/Delete/5
        // Yorumu veritabanından siler
        [HttpPost, ActionName("Delete")] // POST isteği için "Delete" aksiyon adını kullan
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id); // Yorumu Id'ye göre bul
            if (comment != null)
            {
                _context.Comments.Remove(comment); // Yorumu silmek için işaretle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
        }

        // Yorumun veritabanında var olup olmadığını kontrol eden yardımcı metot
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}