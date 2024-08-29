using Food_Explorer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class ProductInfoController : Controller
    {
        public async Task <IActionResult>Product(int id)
        {
            var product = await new Repository<Product>().GetByIdAsync(id);
            return View(product);
        }
        public IActionResult ProductAdmin()
        {
            return View();
        }
    }
}
