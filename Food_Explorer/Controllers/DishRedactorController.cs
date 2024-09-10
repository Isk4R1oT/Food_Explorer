using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Builders;
using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class DishRedactorController : Controller
    {
        public IActionResult AddDish()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDish(IFormFile? image, string name, int category, 
            string ingridients, string price, string description)
        {
            //проверка на пустоту
            if (image == null || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ingridients)
                || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(description))
            {
                return View();
            }
            //base64-строка из файла(картинки)
            string base64image;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                base64image = Convert.ToBase64String(fileBytes);
            }
            //билдинг
            IProductBuilder productBuilder = new ProductBuilder((ProductType)category);
            Product newProduct = productBuilder
                .Name(name)
                .Description(description)
                .Ingredients(ingridients)
                .Price(int.Parse(price))
                .Quantity(999)
                .Image(base64image)
                .Create();
            //добавление
            await new Repository<Product>().CreateAsync(newProduct);
            return RedirectToAction("CatalogAdmin", "Home");
        }

        public async Task<IActionResult> EditDish(int id)
        {
            var product = await new Repository<Product>().GetByIdAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditDish(int id, IFormFile? image, string name, int category,
            string ingridients, string price, string description)
        {
            //проверка на пустоту
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ingridients)
                || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(description))
            {
                //return View();
            }

            var editableProduct = await new Repository<Product>().GetByIdAsync(id);

            if (image != null)
            {
                //base64-строка из файла(картинки)
                string base64image;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    base64image = Convert.ToBase64String(fileBytes);
                }
                editableProduct.Image = base64image;
            }
            editableProduct.Name = name;
            editableProduct.Description = description;
            editableProduct.Ingredients = ingridients;
            editableProduct.Price = int.Parse(price);
            editableProduct.ProductType = (ProductType)category;

            await new Repository<Product>().UpdateAsync(editableProduct);
            return RedirectToAction("CatalogAdmin", "Home");
        }
    }
}
