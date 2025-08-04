using Microsoft.AspNetCore.Mvc;

namespace lokantaWebProject.ViewComponents
{
    public class HeroViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Gerekirse veritabanı işlemi yapılır
            return View();
        }
    }   
}
