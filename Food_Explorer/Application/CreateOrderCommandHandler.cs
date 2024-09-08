using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Builders;
using System.Linq;

namespace Food_Explorer.Application
{
	public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
	{
		private readonly IOrderBuilder _orderBuilder;
		private readonly Context _context;

		public CreateOrderCommandHandler(IOrderBuilder orderBuilder,
			Context context)
		{
			_orderBuilder = orderBuilder;
			_context = context;
		}			
        public void Handle(CreateOrderCommand command)
		{
			if(_context.Products.Any(p => command.BasketItems.Any(b => p.Quantity > 0)))
			{
				throw new Exception("Некоторые товары недоступны");
			}
			int totalPrice = command.BasketItems.Sum(b => b.Product.Price * b.Quantity);
			var newOrder = _orderBuilder.AddOrderItem(command.BasketItems)
										.SetAddress(command.Address)
										.SetUser(_context.Users.Find(command.UserId))
										.SetOrderState(0)
										.SetDate()
										.Build();

		}
	}
}
