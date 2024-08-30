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
            // ���������, ���� �� cookie � ��������������� ���������� ������������
            if (Request.Cookies.ContainsKey("anonymousUserId"))
            {
                // �������� ������������� �� cookie
                var anonymousUserId = Request.Cookies["anonymousUserId"];
            }
            else
            {
                // ���������� ����� ������������� � ��������� ��� � cookie
                var newAnonymousUserId = _context.Users.Max(u=>u.Id)+1;
                var user = UserFactory.CreateUser(UserType.Anonym);
                user.Id = newAnonymousUserId;
                await new Repository<User>().CreateAsync(user);
                Response.Cookies.Append("anonymousUserId", newAnonymousUserId.ToString());
            }

            // ��������� ��������
            var products = _context.Products.Where(x => x.Quantity > 0).ToList();

            // ���������� ������������� � ����������
            return View(products);
        }

        public IActionResult CatalogAdmin()
        {
            return View();
        }       
       
    }
}
