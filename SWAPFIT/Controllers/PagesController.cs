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
            return View(); 
        }
        [HttpGet]
        public IActionResult PrivacyPolicy() => View();

        [HttpGet]
        public IActionResult InspectionPolicy() => View();

        [HttpGet]
        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Terms() => View();
    }

}
