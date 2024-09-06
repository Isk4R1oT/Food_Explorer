using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Data_Access_Layer.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Food_Explorer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IJwtProvider _jwtProvider;
		private readonly IGenericRepository<User> _repository; // Предполагается, что у вас есть репозиторий для работы с пользователями
		private readonly IPasswordHasher _passwordHasher;

		public AccountController(IJwtProvider jwtProvider, IGenericRepository<User> repository, IPasswordHasher passwordHasher)
		{
			_jwtProvider = jwtProvider;
			_repository = repository;
			_passwordHasher = passwordHasher;
		}

		[HttpPost("GetToken")]
		public async Task<IActionResult> GetToken([FromBody] LoginRequest loginRequest)
		{
			// Получаем пользователя по email
			var user = await _repository.GetByEmail(loginRequest.Email);
			if (user == null || !_passwordHasher.Verify(loginRequest.Password, user.Password))
			{
				return Unauthorized("Неверные учетные данные");
			}

			// Генерируем токен для пользователя
			var token = _jwtProvider.GenerateToken(user);

			return Ok(new { Token = token });
		}
	}

	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}