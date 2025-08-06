using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using lokantaWebProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly AdminDbContext _context;

    public HomeController(AdminDbContext context)
    {
        _context = context;
    }

    // Anasayfa
    public async Task<IActionResult> Index()
    {
        var homePageViewModel = new HomePageViewModel
        {
            AboutSectionData = new AboutViewModel
            {
                Vision = await _context.Abouts.FirstOrDefaultAsync(a => a.Title == "Vizyonumuz"),
                Mission = await _context.Abouts.FirstOrDefaultAsync(a => a.Title == "Misyonumuz")
            },
            MenuCategories = await _context.Categories
                                           .Include(c => c.MenuItems)
                                           .ToListAsync(),
            TeamMembers = await _context.TeamMembers.ToListAsync(),
            GalleryImages = await _context.GalleryImages.ToListAsync(),
            Comments = await _context.Comments.ToListAsync(),
            ContactInfoData = await _context.ContactInfos.FirstOrDefaultAsync(),
            Message = new Message()
        };

        return View(homePageViewModel);
    }

    // Hakkımızda
    public async Task<IActionResult> About()
    {
        var model = new AboutViewModel
        {
            Vision = await _context.Abouts.FirstOrDefaultAsync(a => a.Title == "Vizyonumuz"),
            Mission = await _context.Abouts.FirstOrDefaultAsync(a => a.Title == "Misyonumuz")
        };
        return View(model);
    }

    // Menü
    public async Task<IActionResult> Menu()
    {
        var categories = await _context.Categories
                                       .Include(c => c.MenuItems)
                                       .ToListAsync();
        return View(categories);
    }

    // Galeri
    public async Task<IActionResult> Gallery()
    {
        var images = await _context.GalleryImages.ToListAsync();
        return View(images);
    }

    // Yorumlar
    public async Task<IActionResult> Comments()
    {
        var comments = await _context.Comments.ToListAsync();
        return View(comments);
    }

    // İletişim
    public async Task<IActionResult> Contact()
    {
        var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
        return View(contactInfo);
    }

    // Mesaj gönderme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(Message message)
    {
        if (ModelState.IsValid)
        {
            message.SentDate = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Content("OK");
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage);
        return BadRequest(string.Join(", ", errors));
    }
}
