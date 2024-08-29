using Food_Explorer.Entities;
using Food_Explorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Food_Explorer.Controllers
{
    public class SingController : Controller
    {
        private readonly Context _context;

        public SingController(Context context)
        {
            _context = context;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            // Находим пользователя по email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                // Проверяем, совпадают ли пароли
                if (PasswordHash.CheckPass(user,password))
                {
                    // Сохраняем идентификатор пользователя в сессию
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    // Определяем, куда перенаправить пользователя
                    if (user.UserType == UserType.Client)
                    {
                        return RedirectToAction("Catalog", "Home");
                    }
                    else
                    {
                        return RedirectToAction("CatalogAdmin", "Home");
                    }
                }
            }

            // Если пользователь не найден или пароли не совпадают, возвращаем ошибку
            ModelState.AddModelError("", "Неверный email или пароль");
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string name, string email, string password)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.WriteAsync($"<h1 style='margin: 20px 0px 0px 300px'>Вы зарегистрировались</h1>");

            // Проверяем, есть ли пользователь с таким email
            var existingClient = await _context.Users.FirstOrDefaultAsync(c => c.Email == email);
            if (existingClient != null)
            {
                // Если пользователь уже существует, проверяем, был ли он ранее анонимным
                if (existingClient.UserType == UserType.Anonym)
                {
                    // Обновляем тип пользователя на "Client"
                    existingClient.UserType = UserType.Client;
                    existingClient.Name = name;
                    existingClient.Password = PasswordHash.ComputeSHA256Hash(password);
                    await _context.SaveChangesAsync();

                    // Сохраняем идентификатор пользователя в сессию
                    HttpContext.Session.SetInt32("UserId", existingClient.Id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Если пользователь не был анонимным, возвращаем ошибку
                    throw new Exception($"Пользователь с email {email} уже существует.");
                }
            }
            else
            {
                // Если пользователь не существует, создаем нового
                var client = new UserBuilder(UserType.Client)
                    .Name(name)
                    .Email(email)
                    .Password(PasswordHash.ComputeSHA256Hash(password))
                    .Create();

                await _context.Users.AddAsync(client);
                await _context.SaveChangesAsync();

                // Сохраняем идентификатор пользователя в сессию

                HttpContext.Session.SetInt32("UserId", client.Id);

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
