using Microsoft.EntityFrameworkCore;

namespace Food_Explorer.Entities
{    
    public class Context : DbContext
    {
        public Context() : base() { }

        DbSet<Product> Products { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-M0BO597\SQLEXPRESS;
                                          Initial Catalog=Food_Explorer;Integrated Security=True;
                                          Encrypt=True;Trust Server Certificate=True");
        }
    }
}
