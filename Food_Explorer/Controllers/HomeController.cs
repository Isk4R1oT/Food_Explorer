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
using System.Linq;
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
		private readonly UserServes _userServes;

		public HomeController(
			ILogger<HomeController> logger,
			Context context,
			IGenericRepository<User> repository,
			IJwtProvider jwtProvider,
			IOptions<JWTOptions> jwtOptions,
			UserServes userServes)
		{
			_logger = logger;
			_context = context;
			_repository = repository;
			_jwtProvider = jwtProvider;
			_options = jwtOptions.Value;
			_userServes = userServes;
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
