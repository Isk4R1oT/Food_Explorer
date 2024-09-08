using Food_Explorer.Application;
using Microsoft.AspNetCore.Mvc;

namespace Food_Explorer.Controllers
{
    public class CreateOrderController : Controller
    {
        private readonly IMediator _mediator;

        public CreateOrderController(IMediator mediator) => _mediator = mediator;                            
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderCommand command)
        {
            try
            {
                _mediator.Send(command);
                return Ok("Заказ успешно создан");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
