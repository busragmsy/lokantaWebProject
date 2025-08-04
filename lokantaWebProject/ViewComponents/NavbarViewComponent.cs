using Microsoft.AspNetCore.Mvc;

namespace lokantaWebProject.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Gerekirse veritabanı işlemi yapılır
            return View();
        }
    }
}
