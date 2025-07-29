using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using lokantaWebProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace lokantaWebProject.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdminDbContext _context;
        private readonly UserManager<Admin> _userManager;

        public HomeController(ILogger<HomeController> logger, AdminDbContext context, UserManager<Admin> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() 
        {
            ViewBag.MessageCount = await _context.Messages.CountAsync(); 
            ViewBag.AdminCount = await _context.Users.CountAsync(); 
            ViewBag.MenuCount = await _context.MenuItems.CountAsync(); 
            ViewBag.UnreadMessagesCount = await _context.Messages.Where(m => !m.IsRead).CountAsync();
            ViewBag.LatestMenuItems = await _context.MenuItems
                                                   .OrderByDescending(mi => mi.Id) 
                                                   .Take(10) // Son 10 
                                                   .ToListAsync();

            ViewBag.LatestLoginLogs = await _context.AdminLoginLogs
                                                    .OrderByDescending(log => log.LoginTime)
                                                    .Take(10) // Son 10 log
                                                    .ToListAsync();

            ViewData["GalleryImages"] = await _context.GalleryImages.ToListAsync();
            ViewData["ToDos"] = await _context.ToDos.OrderByDescending(x => x.Id).ToListAsync();

            return View();

        }

        [HttpPost]
        public IActionResult Add(string task)
        {
            if (!string.IsNullOrWhiteSpace(task))
            {
                var newTodo = new ToDo
                {
                    Task = task,
                    IsCompleted = false
                };
                _context.ToDos.Add(newTodo);
                _context.SaveChanges(); 
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var todo = await _context.ToDos.FindAsync(id);
            if (todo != null)
            {
                todo.IsCompleted = !todo.IsCompleted;
                await _context.SaveChangesAsync(); 
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.ToDos.FindAsync(id);
            if (todo != null)
            {
                _context.ToDos.Remove(todo);
                await _context.SaveChangesAsync(); 
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}