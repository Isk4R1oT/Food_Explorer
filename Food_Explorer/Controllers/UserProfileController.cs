using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
