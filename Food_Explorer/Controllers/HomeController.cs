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
            if (!Request.Cookies.ContainsKey("anonymousUserId"))
            {
                // �������� ������������� �� cookie
                var anonymousUserId = Request.Cookies["anonymousUserId"];
            }
            else
            {
                // ���������� ����� ������������� � ��������� ��� � cookie
                var user = UserFactory.CreateUser(UserType.Anonym);

                // ��������� ������������ � �����������
                await new Repository<User>().CreateAsync(user);

                // ��������� ���������, ����� �������� ������������� ��������������� Id
                await _context.SaveChangesAsync(); // ���������, ��� SaveChangesAsync ���������� � ����� ������ CreateAsync

                // ������ ����������� Id ������������ �������� ����
                Response.Cookies.Append("anonymousUserId", user.Id.ToString());
            }

            // ��������� ��������
            var products = await _context.Products.Where(x => x.Quantity > 0).ToListAsync();

            // ���������� ������������� � ����������
            return View(products);
        }

        public IActionResult CatalogAdmin()
        {
            return View();
        }       
       
    }
}
