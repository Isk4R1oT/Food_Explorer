using Food_Explorer.Models;
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
        DbSet<Address> Address { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-M0BO597\SQLEXPRESS;
                                          Initial Catalog=Food_Explorer;Integrated Security=True;
                                          Encrypt=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          /*  modelBuilder.Entity<User>()
                .HasOne(u => u.Basket)
                .WithOne(b => b.User)
                .HasPrincipalKey<User>(u => u.Id)
                .HasForeignKey<Basket>(b => b.Id);
*/
            modelBuilder.Entity<BasketItem>()
            .HasOne(bi => bi.Product)
            .WithOne(p => p.BasketItem)
            .HasPrincipalKey<Product>(p=>p.Id)
            .HasForeignKey<BasketItem>(bi => bi.Id);

            modelBuilder.Entity<Product>()
               .HasDiscriminator<ProductType>("ProductType")
               .HasValue<Salad>(ProductType.Salad)
               .HasValue<Soup>(ProductType.Soup)
               .HasValue<Entree>(ProductType.Entree)
               .HasValue<Drink>(ProductType.Drink);

            modelBuilder.Entity<User>()
                .HasDiscriminator<UserType>("UserType")
                .HasValue<Client>(UserType.Client)
                .HasValue<Admin>(UserType.Admin);
        }

    }
}
