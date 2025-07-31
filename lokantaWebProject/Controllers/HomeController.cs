using Microsoft.AspNetCore.Mvc;

namespace lokantaWebProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
