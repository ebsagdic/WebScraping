using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Dto_s;
using WebScraping.Core.Models;

namespace WebScraping.Core.Services
{
    public interface IOrderService
    {
        Task<CustomResponseDto<OrderDto>> GetByIdAsync(int id);
        Task<CustomResponseDto<IEnumerable<OrderDto>>> GetAllAsync();
        Task<CustomResponseDto<CreateOrderDto>> AddAsync(CreateOrderDto order);
        Task<CustomResponseDto<NoContentDto>> Remove(OrderDto order);
    }
}
