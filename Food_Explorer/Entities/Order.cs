using System.ComponentModel.DataAnnotations.Schema;

namespace Food_Explorer.Entities
{
    public enum OrderStateNum
    {
        OrgerInAssembled = 0,
        OrderInTransitState = 1,
        OrderDelivered = 2,
    }
    public class Order
    {
        public int Id { get; set; }

        /*public Adress Adress { get; set; }*/

        public OrderStateNum OrderStateNum { get; set; }

        [NotMapped]
        public IOrderState OrderState { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public bool IsComplited { get; set; } = false;

        public User User { get; set; }

        public DateOnly Date {  get; set; }

        /*public Payment Payment { get; set; }*/

    }
    
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }

    
    public interface IOrderState
    {
        void ProcessOrder(Order order);
    }

    /// <summary>
    /// Заказ собираеться
    /// </summary>
    public class OrgerInAssembled : IOrderState
    {
        public void ProcessOrder(Order order)
        {
            order.OrderState = new OrderInTransitState();
        }

    }
    /// <summary>
    /// Заказ доставляеться
    /// </summary>
    public class OrderInTransitState : IOrderState
    {
        public void ProcessOrder(Order order)
        {
            order.OrderState = new OrderDelivered();
        }
    }

    /// <summary>
    /// Заказ доставлен
    /// </summary>
    public class OrderDelivered : IOrderState
    {
        public void ProcessOrder(Order order)
        {
            order.IsComplited = true;
        }
    }
    
}
