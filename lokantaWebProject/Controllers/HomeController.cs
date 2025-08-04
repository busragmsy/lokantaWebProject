using lokantaWebProject.Context;
using lokantaWebProject.Entities;
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

    public IActionResult Index()
    {
        var aboutItems = _context.Abouts.ToList(); // Vizyon ve Misyon birden fazla kayıt olabilir
        ViewBag.AboutItems = aboutItems;
        return View();
    }
}
