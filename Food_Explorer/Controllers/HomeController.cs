using Food_Explorer.Entities;
using Food_Explorer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Food_Explorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddDish()
        {
            return View();
        }
        public IActionResult EditDish()
        {
            return View();
        }
        public IActionResult Basket()
        {
            return View();
        }
        public IActionResult Catalog()
        {
            return View();
        }
        public IActionResult CatalogAdmin()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult ProductAdmin()
        {
            return View();
        }

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
        }

    }
}
