using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Data_Access_Layer.JWT;
using Food_Explorer.Models;
using Food_Explorer.Data_Access_Layer;

namespace Food_Explorer.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IJwtProvider _jwtProvider;
		private readonly IGenericRepository<User> _userRepository;

		public AuthController(IConfiguration configuration, 
			IJwtProvider jwtProvider,
			IGenericRepository<User> genericRepository)
		{
			_configuration = configuration;
			_jwtProvider = jwtProvider;
			_userRepository = genericRepository;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel login)
		{		
			if (_userRepository.IsValidUser(login.Email, login.Password))
			{
				var token = _jwtProvider.GenerateToken(new Client { Email = login.Email });
				return Ok(new { Token = token });
			}

			return Unauthorized();
		}

		
	}
}