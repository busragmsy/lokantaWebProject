using lokantaWebProject.Context;
using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

public class FooterViewComponent : ViewComponent
{
    private readonly AdminDbContext _context;
    public FooterViewComponent(AdminDbContext context) => _context = context;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _context.ContactInfos.FirstOrDefaultAsync();
        return View(model);
    }
}
