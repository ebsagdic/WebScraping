using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebScraping.Core.Dto_s;
using WebScraping.Core.Services;

namespace WebScraping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllAsync();
            return ActionResultInstance(response);
            //return new ObjectResult(response) {  StatusCode= response.StatusCode};
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await _orderService.GetByIdAsync(id);
            return ActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto orderDto)
        {
            var response = await _orderService.AddAsync(orderDto);
            return ActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            OrderDto orderDto = new OrderDto { Id = id };
            var response = await _orderService.Remove(orderDto);
            return ActionResultInstance(response);
        }

    }
}
