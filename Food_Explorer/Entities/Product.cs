namespace Food_Explorer.Entities
{
    
    public enum ProductType
    {
        Salad = 0,
        Soup = 1,
        Entree = 2,
        Drink = 3,
    }


    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Ingredients { get; set; }

        public int Price { get; set; }

        public ProductType ProductType { get; set; }

        public int Quantity { get; set; }

        //Состояние наличия продуктов

        public byte[] Image { get; set; }

        public BasketItem BasketItem { get; set; }

        public IEnumerable<OrderItem> OrderItem { get; set; }

    }


    //Состояние наличия продуктов
    public interface IProductState
    {
        void CheckAvailability(Product product);
    }


    public class Salad : Product
    {

    }

    public class Soup : Product
    {

    }

    public class Entree : Product
    {

    }

    public class Drink : Product
    {

    }

    public class ProductFactory
    {
        public static Product CreateProduct(ProductType type)
        {
            switch (type)
            {
                case ProductType.Salad:
                    return new Salad();
                case ProductType.Soup:
                    return new Soup();
                case ProductType.Entree:
                    return new Entree();
                case ProductType.Drink:
                    return new Drink();
                default:
                    throw new ArgumentException($"Invalid product type: {type}");
            }
        }
    }

}
