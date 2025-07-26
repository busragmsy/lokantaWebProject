using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lokantaWebProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AdminDbContext _context;

        public CategoryController(AdminDbContext context)
        {
            _context = context;
        }

        // GET: /Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category) // Güvenlik için Bind kullanıldı
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(e => e.Id == category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            // İlişkili MenuItem'lar varsa silinmeyeceğini belirtmek için kontrol eklenebilir
            // var hasMenuItems = await _context.MenuItems.AnyAsync(mi => mi.CategoryId == id);
            // if (hasMenuItems)
            // {
            //     ModelState.AddModelError("", "Bu kategoriye ait menü öğeleri olduğu için silinemez.");
            //     return View(category);
            // }

            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null) // Kontrol eklendi
            {
                try
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex) // İlişkili kayıt hatasını yakalamak için
                {
                    // Hatanın detaylarını loglayın veya kullanıcıya gösterin
                    ModelState.AddModelError("", "Bu kategoriye bağlı menü öğeleri olduğu için silinemez. Lütfen önce menü öğelerini silin.");
                    return View(category); // Aynı sayfada hata mesajı göster
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}