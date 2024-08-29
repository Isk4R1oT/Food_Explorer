using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class ProductInfoController : Controller
    {
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult ProductAdmin()
        {
            return View();
        }
    }
}
