using Food_Explorer.Data_Access_Layer.Builders;
using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Data_Access_Layer.JWT;
using Food_Explorer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Food_Explorer.Data_Access_Layer
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool Verify(string password, string hashPassword);
    }
    public class PasswordHash : IPasswordHasher
    {
        public string Generate(string input) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(input);
        public bool Verify(string password, string hashPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
    }

    public class UserServes
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IGenericRepository<User> _repository;
        private readonly IJwtProvider _ijwtProvider;

        public UserServes(IPasswordHasher passwordHasher,
            IGenericRepository<User> repository,
            IJwtProvider ijwtProvider)
        {
            _passwordHasher = passwordHasher;
            _repository = repository;
            _ijwtProvider = ijwtProvider;
        }
        public async Task DeAnonim(User existingClient, string name, string password)
        {
            existingClient = existingClient as Client;
            existingClient.UserType = UserType.Client;
            existingClient.Name = name;
            existingClient.Password = _passwordHasher.Generate(password);
            await _repository.UpdateAsync(existingClient);
        }
        public async Task<User> Registr(string name, string email, string password)
        {
            var client = new UserBuilder(UserType.Client)
                  .Name(name)
                  .Email(email)
                  .Password(_passwordHasher.Generate(password))
                  .Create();
            await _repository.CreateAsync(client);
            return client;

        }

		public async Task<string> Login(string email, string password)
		{
			var user = await _repository.GetByEmail(email);
			if (user == null || !_passwordHasher.Verify(password, user.Password))
			{
				throw new Exception("Login failed");
			}
			var token = _ijwtProvider.GenerateToken(user);
			return token;
		}
	}
    
}
