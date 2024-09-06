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
				// Создание анонимного пользователя
				var user = UserFactory.CreateUser(UserType.Anonym);
				await _repository.CreateAsync(user);
				await _context.SaveChangesAsync();
				var jwtToken = _jwtProvider.GenerateToken(user);

				return Ok(new { Token = jwtToken });
			}
			else
			{
				var principal = ValidateToken(tokenRequest.Token);
				if (principal != null)
				{
					var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
					var existingUser = await _repository.GetByIdAsync(int.Parse(userId));
					return existingUser != null ? Ok(existingUser) : NotFound("Пользователь не найден.");
				}
				else
				{
					return Unauthorized("Недействительный токен");
				}
			}
		}
		// Метод для валидации токена
		private ClaimsPrincipal ValidateToken(string token)
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
				return tokenHandler.ValidateToken(token, validationParameters, out _);
			}
			catch
			{
				return null; // Токен недействителен
			}
		}
		public async Task<IActionResult> Catalog()
		{

			// Загружаем продукты
			var products = await _context.Products.Where(x => x.Quantity > 0).ToListAsync();

			// Возвращаем представление с продуктами
			return View(products);
		}

		public IActionResult CatalogAdmin()
		{
			return View();
		}

	}
	public class TokenRequest
	{
		public string Token { get; set; }
	}
}
