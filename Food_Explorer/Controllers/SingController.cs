using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class SingController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            //some logic
            if (true)
            {
                return RedirectToAction("Catalog", "Home");
            }
            else
            {
                return RedirectToAction("CatalogAdmin", "Home");
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public void SignUp(string name, string email, string password)
        {
            //some logic
            Response.ContentType = "text/html; charset=utf-8";
            Response.WriteAsync($"<h1 style='margin: 20px 0px 0px 300px'>Вы зарегистрировались</h1>");
            //добавить юзера
        }

    }
}
