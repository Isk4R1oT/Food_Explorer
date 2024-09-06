using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Food_Explorer.Controllers
{
    public class BasketController : Controller
    {
        private readonly Context _context;

        public BasketController(Context context)
        {
            _context = context;
        }

        public IActionResult Basket()
        {
            // Проверяем, авторизован ли пользователь
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                // Если пользователь авторизован, получаем его корзину
                var basket = _context.Baskets.Include(b => b.BasketItems).ThenInclude(c => c.Product)
                    .First(x => x.User.Id == HttpContext.Session.GetInt32("UserId").Value);
                return View(basket);
            }
            else
            {
                // Если пользователь не авторизован, получаем корзину анонимного пользователя
                var anonymousUserId = Request.Cookies["anonymousUserId"];
                if (!string.IsNullOrEmpty(anonymousUserId))
                {
                    var basket = _context.Baskets.Include(b => b.BasketItems).ThenInclude(c => c.Product)
                        .FirstOrDefault(x => x.User.Id == Convert.ToInt32(anonymousUserId));
                    if (basket != null)
                    {
                        return View(basket);
                    }
                }

                // Если нет корзины анонимного пользователя, возвращаем пустую корзину
                return View(new Basket { BasketItems = new List<BasketItem>() });
            }
        }

        public async Task<IActionResult> AddToBasket(int id)
        {
            var product = await new Repository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Получаем текущего клиента (авторизованного или анонимного)
            var currentClient = await GetCurrentClient();

            if (currentClient != null)
            {
                // Проверяем, есть ли у клиента корзина
                var basket = await GetOrCreateBasket(currentClient.Id);

                // Добавляем товар в корзину
                AddProductToBasket(basket, product);

                await SaveBasket(basket);

                return RedirectToAction("AddToBasket", "Basket");
            }
            else
            {
                return RedirectToAction("UserProfile", "UserMenu");
            }
        }

        private async Task<User> GetCurrentClient()
        {
            // Проверяем, авторизован ли пользователь
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                // Если пользователь авторизован, возвращаем его
                return await _context.Users.FindAsync(HttpContext.Session.GetInt32("UserId").Value);
            }
            else
            {
                // Если пользователь не авторизован, проверяем наличие анонимного пользователя
                var anonymousUserId = Request.Cookies["anonymousUserId"];
                if (!string.IsNullOrEmpty(anonymousUserId))
                {
                    // Если есть анонимный пользователь, возвращаем его
                    return await _context.Users.FindAsync(Convert.ToInt32(anonymousUserId));
                }
            }

            // Если нет ни авторизованного, ни анонимного пользователя, возвращаем null
            return null;
        }

        private async Task<Basket> GetOrCreateBasket(int userId)
        {

            // Проверяем, есть ли у пользователя корзина
            var basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.User.Id == userId);

            if (basket == null)
            {
                // Если корзины нет, создаем новую
                basket = new Basket
                {
                    BasketItems = new List<BasketItem>(),
                    User = await _context.Users.FindAsync(userId)
                };
            }

            return basket;
        }

        private void AddProductToBasket(Basket basket, Product product)
        {
            // Проверяем, есть ли элемент корзины с таким же продуктом
            var basketItem = basket.BasketItems.FirstOrDefault(bi => bi.Product.Id == product.Id);
            if (basketItem == null)
            {
                // Если элемента нет, добавляем новый
                basketItem = new BasketItem
                {
                    Product = product,
                    Basket = basket,
                    Quantity = 1
                };
                basket.BasketItems.Add(basketItem);
            }
            else
            {
                // Увеличиваем количество товара в корзине
                basketItem.Quantity++;
            }
        }

        private async Task SaveBasket(Basket basket)
        {
            // Проверяем, есть ли корзина в базе данных
            var existingBasket = await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.Id == basket.Id);

            if (existingBasket == null)
            {
                // Если корзины нет, добавляем новую
                _context.Baskets.Add(basket);
            }
            else
            {
                // Если корзина существует, обновляем её
                _context.Entry(existingBasket).CurrentValues.SetValues(basket);

                // Обновляем элементы корзины
                foreach (var basketItem in basket.BasketItems)
                {
                    var existingBasketItem = existingBasket.BasketItems.FirstOrDefault(bi => bi.Id == basketItem.Id);
                    if (existingBasketItem == null)
                    {
                        // Если элемента нет, добавляем новый
                        existingBasket.BasketItems.Add(basketItem);
                    }
                    else
                    {
                        // Если элемент существует, обновляем его
                        _context.Entry(existingBasketItem).CurrentValues.SetValues(basketItem);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
