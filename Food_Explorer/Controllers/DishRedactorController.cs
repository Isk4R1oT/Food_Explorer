using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class DishRedactorController : Controller
    {
        public IActionResult AddDish()
        {
            return View();
        }
        public IActionResult EditDish()
        {
            return View();
        }
    }
}
