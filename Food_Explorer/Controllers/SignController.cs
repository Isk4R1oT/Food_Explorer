using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Data_Access_Layer.JWT;
using Food_Explorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Food_Explorer.Controllers
{
	public class SignController : Controller
	{
		private readonly Context _context;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IJwtProvider _jwtProvider;

		public SignController(Context context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
		{
			_context = context;
			_passwordHasher = passwordHasher;
			_jwtProvider = jwtProvider;
		}

		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(string email, string password)
		{			
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

			if (user != null)
			{			
				if (_passwordHasher.Verify(password, user.Password))
				{					
					HttpContext.Session.SetInt32("UserId", user.Id);
					
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
			var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (existingUser != null)
			{
				ModelState.AddModelError("", "Пользователь с таким email уже существует.");
				return View();
			}

			var newUser = new Client
			{
				Name = name,
				Email = email,
				Password = _passwordHasher.Generate(password), 
				UserType = UserType.Client,
				CreatedAt = DateTime.Now
			};

			var token = _jwtProvider.GenerateToken(newUser);

			newUser.Token = token;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Sign", "SignIn");
        }

		public IActionResult Exit()
		{
            HttpContext.Session.SetInt32("UserId", 0);
            return RedirectToAction("SignIn", "Sign");
        }
    }
}