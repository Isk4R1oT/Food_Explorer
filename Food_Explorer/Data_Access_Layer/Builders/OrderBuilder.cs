using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Models;
using System.Net;

namespace Food_Explorer.Data_Access_Layer.Builders
{
    public interface IOrderBuilder
    {
        IOrderBuilder AddOrderItem(ICollection<BasketItem>orderItem);
        IOrderBuilder SetAddress(Address address);
        IOrderBuilder SetOrderState(OrderStateNum orderStateNum);
        IOrderBuilder SetUser(User user);
        IOrderBuilder SetDate();
        Order Build();
    }

    public class OrderBuilder : IOrderBuilder
    {
        private readonly Order _order;

        public OrderBuilder()
        {
            _order = new Order();
        }

        public IOrderBuilder AddOrderItem(ICollection<BasketItem>orderItem)
        {
            if (_order.OrderItems == null)
                _order.OrderItems = new List<OrderItem>();

            _order.OrderItems.Add((OrderItem)orderItem);
            return this;
        }

        public IOrderBuilder SetAddress(Address address)
        {
            _order.Address = address;
            return this;
        }

        public IOrderBuilder SetOrderState(OrderStateNum orderStateNum)
        {
            _order.OrderStateNum = orderStateNum;
            _order.OrderState = GetOrderStateInstance(orderStateNum);
            return this;
        }

        public IOrderBuilder SetUser(User user)
        {
            _order.User = user;
            return this;
        }

        public IOrderBuilder SetDate()
        {
            _order.Date = DateOnly.FromDateTime(DateTime.Now);
            return this;
        }
        public Order Build()
        {
            return _order;
        }

        private IOrderState GetOrderStateInstance(OrderStateNum orderStateNum)
        {
            switch (orderStateNum)
            {
                case OrderStateNum.OrgerInAssembled:
                    return new OrgerInAssembled();
                case OrderStateNum.OrderInTransitState:
                    return new OrderInTransitState();
                case OrderStateNum.OrderDelivered:
                    return new OrderDelivered();
                default:
                    throw new ArgumentException($"Invalid OrderStateNum: {orderStateNum}");
            }
        }
    }

}
