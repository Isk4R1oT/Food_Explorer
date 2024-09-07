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
				// �������� ������ ���������� ������������
				var user = UserFactory.CreateUser(UserType.Anonym);
				var jwtToken = _jwtProvider.GenerateToken(user);
				user.Token = jwtToken;
				/*SetAuthToken(jwtToken);*/
				await _repository.CreateAsync(user);				
				return Ok(new { Token = jwtToken });
			}

			try
			{
				// ��������� ���������������� ������
				var existingUser = await _repository.GetByToken(tokenRequest.Token);
				if (existingUser != null)
				{
					// ���� ����� ������������, ���������� ������������� ������������
					var claimsPrincipal = ValidateToken(tokenRequest.Token, existingUser);
					if (claimsPrincipal != null)
					{
						// ��������� �����, ���� ��� ���������� (��������, ���� �� ����� �������)
						existingUser.Token = _jwtProvider.GenerateToken(existingUser);
						await _repository.UpdateAsync(existingUser); // ���������� ������������ � ����� �������
						return Ok(existingUser); // ���������� ���������� � ������������
					}
					else
					{
						return Unauthorized("���������������� �����");
					}
				}
				else
				{
					// ���� ������������ � ������� �� ������, ������� ������ ���������� ������������
					var newUser = UserFactory.CreateUser(UserType.Anonym);
					await _repository.CreateAsync(newUser);

					// ��������� ������ ������ ��� ���������� ������������
					var jwtToken = _jwtProvider.GenerateToken(newUser);
					newUser.Token = jwtToken; // ���������� ������
					await _repository.UpdateAsync(newUser); // ���������� ������������ � ����� �������

					return Ok(new { Token = jwtToken });
				}
			}
			catch (Exception ex)
			{
				// ����������� ������
				// _logger.LogError(ex, "������ ��� ��������� ������ ������������.");
				return StatusCode(500, "���������� ������ �������");
			}
		}
		// ����� ��� ��������� ������
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

				// ���������, ������������� �� ID ������������ � ������ ID ������������ � ���� ������
				var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
				if (userIdClaim == null || int.Parse(userIdClaim.Value) != user.Id)
				{
					return null; // ����� ��������������
				}

				return claimsPrincipal;
			}
			catch
			{
				return null; // ����� ��������������
			}
		}
		public IActionResult SetAuthToken(string jwtToken)
		{
			// ������� ��������� ����
			var cookieOptions = new CookieOptions
			{
				Expires = DateTimeOffset.UtcNow.AddDays(7), // ������������� ���� �������� ���� �� 7 ����
				HttpOnly = true, // ��������� ������ � ���� ����� JavaScript
				Secure = true, // ���������, ��� ���� ���������� ������ ����� HTTPS
				SameSite = SameSiteMode.Strict // ��������� �������� SameSite
			};

			// �������� �� null ��� �������
			if (Response == null)
			{
				return StatusCode(500, "Response is null.");
			}

			// ������������� ���� � ������ "AuthToken" � ��������� jwtToken
			Response.Cookies.Append("AuthToken", jwtToken, cookieOptions);
			return Ok("���� �������������� �����������");
		}
		public async Task<IActionResult> Catalog()
		{

			// ��������� ��������
			var products = await _context.Products.Where(x => x.Quantity > 0).ToListAsync();

			// ���������� ������������� � ����������
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
