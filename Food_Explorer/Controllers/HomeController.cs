using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Builders;
using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Data_Access_Layer.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Explorer.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;		
		private readonly Context _context;
		private readonly IGenericRepository<User> _repository;
		private readonly IJwtProvider _jwtProvider;
		private readonly JWTOptions _options;

		public HomeController(
			ILogger<HomeController> logger,
			Context context,
			IGenericRepository<User> repository,
			IJwtProvider jwtProvider,
			IOptions<JWTOptions> jwtOptions)
		{
			_logger = logger;
			_context = context;
			_repository = repository;
			_jwtProvider = jwtProvider;
			_options = jwtOptions.Value;
		}
		[HttpPost]
		public async Task<IActionResult> HandleUserSession([FromBody] TokenRequest tokenRequest)
		{
			if (string.IsNullOrEmpty(tokenRequest.Token))
			{
				// Создание нового анонимного пользователя
				var user = UserFactory.CreateUser(UserType.Anonym);
				var jwtToken = _jwtProvider.GenerateToken(user);
				user.Token = jwtToken;
				/*SetAuthToken(jwtToken);*/
				await _repository.CreateAsync(user);				
				return Ok(new { Token = jwtToken });
			}

			try
			{
				// Валидация предоставленного токена
				var existingUser = await _repository.GetByToken(tokenRequest.Token);
				if (existingUser != null)
				{
					// Если токен действителен, возвращаем существующего пользователя
					var claimsPrincipal = ValidateToken(tokenRequest.Token, existingUser);
					if (claimsPrincipal != null)
					{
						// Обновляем токен, если это необходимо (например, если он скоро истечет)
						existingUser.Token = _jwtProvider.GenerateToken(existingUser);
						await _repository.UpdateAsync(existingUser); // Обновление пользователя с новым токеном
						return Ok(existingUser); // Возвращаем информацию о пользователе
					}
					else
					{
						return Unauthorized("Недействительный токен");
					}
				}
				else
				{
					// Если пользователь с токеном не найден, создаем нового анонимного пользователя
					var newUser = UserFactory.CreateUser(UserType.Anonym);
					await _repository.CreateAsync(newUser);

					// Генерация нового токена для анонимного пользователя
					var jwtToken = _jwtProvider.GenerateToken(newUser);
					newUser.Token = jwtToken; // Сохранение токена
					await _repository.UpdateAsync(newUser); // Обновление пользователя с новым токеном

					return Ok(new { Token = jwtToken });
				}
			}
			catch (Exception ex)
			{
				// Логирование ошибки
				// _logger.LogError(ex, "Ошибка при обработке сессии пользователя.");
				return StatusCode(500, "Внутренняя ошибка сервера");
			}
		}
		// Метод для валидации токена
		private ClaimsPrincipal ValidateToken(string token, User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
				ValidateIssuer = true,
				ValidIssuer = _options.Issuer,
				ValidateAudience = true,
				ValidAudience = _options.Audience,
				ClockSkew = TimeSpan.Zero
			};

			try
			{
				var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

				// Проверяем, соответствует ли ID пользователя в токене ID пользователя в базе данных
				var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
				if (userIdClaim == null || int.Parse(userIdClaim.Value) != user.Id)
				{
					return null; // Токен недействителен
				}

				return claimsPrincipal;
			}
			catch
			{
				return null; // Токен недействителен
			}
		}
		public IActionResult SetAuthToken(string jwtToken)
		{
			// Создаем параметры куки
			var cookieOptions = new CookieOptions
			{
				Expires = DateTimeOffset.UtcNow.AddDays(7), // Устанавливаем срок действия куки на 7 дней
				HttpOnly = true, // Запретить доступ к куки через JavaScript
				Secure = true, // Убедитесь, что куки передаются только через HTTPS
				SameSite = SameSiteMode.Strict // Настройте политику SameSite
			};

			// Проверка на null для отладки
			if (Response == null)
			{
				return StatusCode(500, "Response is null.");
			}

			// Устанавливаем куку с именем "AuthToken" и значением jwtToken
			Response.Cookies.Append("AuthToken", jwtToken, cookieOptions);
			return Ok("Кука аутентификации установлена");
		}
		public async Task<IActionResult> Catalog()
		{

			// Загружаем продукты
			var products = await _context.Products.Where(x => x.Quantity > 0).ToListAsync();

			// Возвращаем представление с продуктами
			return View(products);
		}

		public async Task<IActionResult> CatalogAdmin()
		{
            var products = await _context.Products.Where(x => x.Quantity > 0).ToListAsync();
            return View(products);
		}

	}
	public class TokenRequest
	{
		public string Token { get; set; }
	}
}
