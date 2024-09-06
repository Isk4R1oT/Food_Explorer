using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Food_Explorer.Data_Access_Layer.Entities
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
        public string? Login { get; set; }
        public string? Name { get; set; }

        public abstract UserType UserType { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }

        public Basket? Basket { get; set; }

        public IEnumerable<Order>? Orders { get; set; }

        protected User()
        {
            Orders = new List<Order>();
        }

    }
    public class Client : User
    {
        public override UserType UserType { get; set; } = UserType.Client;
    }
    public class Admin : User
    {
        public override UserType UserType { get; set; } = UserType.Admin;
    }
    public class Anonym : User
    {
        public override UserType UserType { get; set; } = UserType.Anonym;
    }
}
