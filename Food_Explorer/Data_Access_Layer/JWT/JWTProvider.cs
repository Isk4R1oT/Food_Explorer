using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Explorer.Data_Access_Layer.JWT
{
	public interface IJwtProvider
	{
		string GenerateToken(User user);
	}

	public class JWTProvider : IJwtProvider
	{
		private readonly JWTOptions _options;

		public JWTProvider(IOptions<JWTOptions> options)
		{
			_options = options.Value;
		}

		public string GenerateToken(User user)
		{
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
				SecurityAlgorithms.HmacSha256
			);

			var claims = new[]
			{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),			
            new Claim(ClaimTypes.Role, user.UserType.ToString()) // Используем UserType для роли
        };

			var token = new JwtSecurityToken(
				issuer: _options.Issuer,
				audience: _options.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(_options.LiveHours),
				signingCredentials: signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
	public class JWTOptions
	{
		public string SecretKey { get; set; } = string.Empty; // Секретный ключ для подписи токена
		public int LiveHours { get; set; } // Время жизни токена в часах
		public string Issuer { get; set; } = string.Empty; // Издатель токена
		public string Audience { get; set; } = string.Empty; // Аудитория токена
	}
}
