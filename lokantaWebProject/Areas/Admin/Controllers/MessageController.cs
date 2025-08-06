using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ToListAsync, FindAsync, FirstOrDefaultAsync için

namespace lokantaWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class MessageController : Controller
    {
        private readonly AdminDbContext _context; // Veritabanı bağlamı

        public MessageController(AdminDbContext context)
        {
            _context = context; // Dependency Injection ile bağlamı başlat
        }

        // GET: /Message/
        // Tüm mesajları listeleyen ana sayfa
        public async Task<IActionResult> Index()
        {
            // Tüm mesajları gönderilme tarihine göre en yeniden eskiye doğru sırala
            var messages = await _context.Messages.OrderByDescending(m => m.SentDate).ToListAsync();
            return View(messages);
        }

        // GET: /Message/Details/5
        // Belirli bir mesajın detaylarını gösterir ve okundu olarak işaretler
        public async Task<IActionResult> Details(int? id)
        {
            // Id null ise veya mesaj bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            // Mesajı okundu olarak işaretle ve veritabanına kaydet
            if (!message.IsRead)
            {
                message.IsRead = true;
                _context.Update(message);
                await _context.SaveChangesAsync();
            }

            // Mesajı View'e gönder
            return View(message);
        }

        // GET: /Message/Delete/5
        // Belirli bir mesajı silme onay sayfasını gösterir
        public async Task<IActionResult> Delete(int? id)
        {
            // Id null ise veya mesaj bulunamazsa 404 döndür
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            // Mesajı View'e gönder
            return View(message);
        }

        // POST: /Message/Delete/5
        // Mesajı veritabanından siler
        [HttpPost, ActionName("Delete")] // POST isteği için "Delete" aksiyon adını kullan
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Messages.FindAsync(id); // Mesajı Id'ye göre bul
            if (message != null)
            {
                _context.Messages.Remove(message); // Mesajı silmek için işaretle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction(nameof(Index)); // Index sayfasına yönlendir
        }

        // Mesajın veritabanında var olup olmadığını kontrol eden yardımcı metot (Şu an kullanılmıyor ama Edit/Create olsaydı gerekliydi)
        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}