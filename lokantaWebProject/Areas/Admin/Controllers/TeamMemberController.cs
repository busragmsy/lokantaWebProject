using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lokantaWebProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class TeamMemberController : Controller
    {
        private readonly AdminDbContext _context;
        public TeamMemberController(AdminDbContext context)
        {
            _context = context;
        }
        // GET: /TeamMember
        public async Task<IActionResult> Index()
        {
            var members = await _context.TeamMembers.ToListAsync();
            return View(members);
        }

        // GET: /TeamMember/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TeamMember/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: /TeamMember/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: /TeamMember/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamMember member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: /TeamMember/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: /TeamMember/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.TeamMembers.FindAsync(id);
            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /TeamMember/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }
    }
}
