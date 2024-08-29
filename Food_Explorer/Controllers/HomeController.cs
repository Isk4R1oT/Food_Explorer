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
           
        public IActionResult Catalog()
        {
            return View();
        }
        public IActionResult CatalogAdmin()
        {
            return View();
        }       
       
    }
}
