namespace Food_Explorer.Entities
{
    public class Basket
    {
        public int Id { get; set; }

        public IEnumerable<BasketItem> BasketItems { get; set; }

        public User User { get; set; }
    }

    public class BasketItem
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Basket Basket { get; set; }

        public int Quantity { get; set; }
    }

}
