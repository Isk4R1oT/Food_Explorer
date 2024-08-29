using System.Security.Cryptography;
using System.Text;

namespace Food_Explorer.Entities
{

    
    public enum UserType
    {
        Client,
        Admin,
        Anonym
    }

    public abstract class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }

        public  UserType UserType { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }

        public Basket Basket { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        protected User()
        {
            Orders = new List<Order>();
        }

    }
    public class Client : User
    {
        
    }
    public class Admin : User
    {
        
    }
    public class Anonym
    {

    }
    static class PasswordHash
    {
        public static string ComputeSHA256Hash(this string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                // Преобразование массива байтов в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static bool CheckPass(User user, string pass)
        {
            using (var context = new Context())
            {               
                if (user.Password == pass.ComputeSHA256Hash())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
