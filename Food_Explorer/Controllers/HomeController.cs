using Food_Explorer.Entities;
using Food_Explorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Food_Explorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task <IActionResult> Catalog()
        {
            // Проверяем, есть ли cookie с идентификатором анонимного пользователя
            if (Request.Cookies.ContainsKey("anonymousUserId"))
            {
                // Получаем идентификатор из cookie
                var anonymousUserId = Request.Cookies["anonymousUserId"];
            }
            else
            {
                // Генерируем новый идентификатор и добавляем его в cookie
                var newAnonymousUserId = _context.Users.Max(u=>u.Id)+1;
                var user = UserFactory.CreateUser(UserType.Anonym);
                user.Id = newAnonymousUserId;
                await new Repository<User>().CreateAsync(user);
                Response.Cookies.Append("anonymousUserId", newAnonymousUserId.ToString());
            }

            // Загружаем продукты
            var products = _context.Products.Where(x => x.Quantity > 0).ToList();

            // Возвращаем представление с продуктами
            return View(products);
        }

        public IActionResult CatalogAdmin()
        {
            return View();
        }       
       
    }
}
