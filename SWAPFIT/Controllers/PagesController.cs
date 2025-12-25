using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Guide()
        {
            return View();
        }

        public IActionResult CreateArticles()
        {
            return View();
        }
        public IActionResult Phoido()
        {
            return View(); // tìm Views/Pages/Phoido.cshtml
            // hoặc return View("~/Views/Pages/Phoido.cshtml");
        }
    }

}
