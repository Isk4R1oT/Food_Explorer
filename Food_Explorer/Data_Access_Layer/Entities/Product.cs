namespace Food_Explorer.Data_Access_Layer.Entities
{

    public enum ProductType
    {
        Entree,
        Drink,
        Dessert
    }


    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public int Price { get; set; }

        public abstract ProductType ProductType { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }

        public BasketItem BasketItem { get; set; }

        public IEnumerable<OrderItem> OrderItem { get; set; }

    }


    //Состояние наличия продуктов
    public interface IProductState
    {
        void CheckAvailability(Product product);
    }

    public class Entree : Product
    {
        public override ProductType ProductType { get; set; } = ProductType.Entree;
    }

    public class Drink : Product
    {
        public override ProductType ProductType { get; set; } = ProductType.Drink;
    }

    public class Dessert : Product
    {
        public override ProductType ProductType { get; set; } = ProductType.Dessert;
    }

}
