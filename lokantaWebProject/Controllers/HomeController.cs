using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using lokantaWebProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

public class HomeController : Controller
{
    private readonly AdminDbContext _context;

    public HomeController(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Tüm anasayfa verilerini tek bir ViewModel'de topluyoruz.
        var homePageViewModel = new HomePageViewModel
        {
            // About Bölümü verilerini çekme
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
            Comments = await _context.Comments.ToListAsync()
        };

        return View(homePageViewModel);
    }
}
