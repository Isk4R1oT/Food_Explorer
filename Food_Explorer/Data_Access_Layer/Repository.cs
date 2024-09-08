using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Food_Explorer.Data_Access_Layer
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByEmail(string email);
        Task<User> GetByToken(string token);         

        bool IsValidUser(string email,string password);


	}

    public class Repository<T> : IGenericRepository<T> where T : class
    {
        private readonly Context _context;

        public Repository() => _context = new Context();

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByEmail(string email) => await _context.Set<T>().FindAsync(email);

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByToken(string token)=>
            string.IsNullOrEmpty(token)?null: await _context.Users
				.FirstOrDefaultAsync(u => u.Token == token);
	

		public bool IsValidUser(string email, string password)
		{
            bool validEmail = _context.Users.Any(m => m.Email == email);
            if (validEmail)
            {
                var user = _context.Users.Find(email);
                if (new PasswordHash().Verify(password,user.Password))
                {
                    return true;
                }
            }
            return false;
		}
	}
}
