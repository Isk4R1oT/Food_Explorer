using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Basket()
        {
            return View();
        }
    }
}
