using Food_Explorer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Explorer.Entities
{
	public interface IPasswordHasher
	{
		string Generate(string password);
		bool Verify(string password, string hashPassword);
	}	
	public class PasswordHash : IPasswordHasher
	{
		public  string Generate(string input) =>
			BCrypt.Net.BCrypt.EnhancedHashPassword(input);
		public  bool Verify(string password, string hashPassword) =>
			BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
	}

	public class UserServes
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly Repository<User> _repository;
		private readonly IJwtProvider _ijwtProvider;

		public UserServes(IPasswordHasher passwordHasher,
			Repository<User> repository,
			IJwtProvider ijwtProvider)
        {
            _passwordHasher = passwordHasher;
			_repository = repository;
			_ijwtProvider = ijwtProvider;
		}
		public async Task DeAnonim(User existingClient,string name,string password)
		{
			existingClient = existingClient as Client;
			existingClient.UserType = UserType.Client;
			existingClient.Name = name;
			existingClient.Password = _passwordHasher.Generate(password);
			await _repository.UpdateAsync(existingClient);
		}
		public async Task Registr(string name, string email,string password)
		{
			var client = new UserBuilder(UserType.Client)
				  .Name(name)
				  .Email(email)
				  .Password(_passwordHasher.Generate(password))
				  .Create();
			await _repository.CreateAsync(client);			

		}

		public async Task<string> Login(string email, string password)
		{
			var user = await _repository.GetByEmail(email);
			if(!_passwordHasher.Verify(password,user.Password))
			{
				throw new Exception("Login failed");
			}
			var token = _ijwtProvider.GenerateToken(user);
			return token;
		}
    }

	public interface IJwtProvider
	{
		string GenerateToken(User user);
	}
	class JWTProvider : IJwtProvider 
	{
		private readonly JWTOptions _options;
        public JWTProvider(IOptions<JWTOptions> options)
        {
			_options = options.Value;
        }
        public string GenerateToken(User user)
		{
			//создаем секретный ключ по выбранному алгоритму
			var singCredentians = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
				 SecurityAlgorithms.HmacSha256);


			Claim[] claims = [new("userId", user.Id.ToString())];
			//создаем токен
			var token = new JwtSecurityToken(
				claims: claims,
				signingCredentials: singCredentians,
				expires: DateTime.UtcNow.AddHours(_options.LiveHours));	

			//создаем из токена строку 
			return new JwtSecurityTokenHandler().WriteToken(token);	
		}
	}
	class JWTOptions
	{
		public string SecretKey { get; set; } = string.Empty;
		public int LiveHours { get; set; }
	}
}
