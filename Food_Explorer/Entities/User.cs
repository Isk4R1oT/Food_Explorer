namespace Food_Explorer.Entities
{

    
    public enum UserType
    {
        Client,
        Admin
    }

    public abstract class User
    {
        public string Login { get; set; }
        public string Name { get; set; }

        public  UserType UserType { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }

        public Basket Basket { get; set; }

        public IEnumerable<Order> Orders { get; set; }       
        
    }
    public class Client : User
    {
        
    }
    public class Admin : User
    {
        
    }
    

}
