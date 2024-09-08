using Food_Explorer.Data_Access_Layer.Entities;
using Food_Explorer.Models;

namespace Food_Explorer.Application
{
	public class CreateOrderCommand
	{
		public int UserId { get; set; }
		public ICollection<BasketItem> BasketItems { get; set; }
		public Address Address { get; set; }
	}
}
