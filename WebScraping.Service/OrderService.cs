using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Dto_s;
using WebScraping.Core.Models;
using WebScraping.Core.Repositories;
using WebScraping.Core.Services;
using WebScraping.Core.UnitOfWork;

namespace WebScraping.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CustomResponseDto<CreateOrderDto>> AddAsync(CreateOrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.CreatedDate = DateTime.Now;
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<CreateOrderDto>.Success(200, orderDto);
        }

        public async Task<CustomResponseDto<IEnumerable<OrderDto>>> GetAllAsync()
        {
            var order =  await _orderRepository.GetAllAsync();
            if(!order.Any())
            {
                var errors = new List<string>();
                errors.Add("Repoda Order bulunmuyor.");
                return CustomResponseDto<IEnumerable<OrderDto>>.Fail(404, errors);
            }
            var orderDto = _mapper.Map<IEnumerable<OrderDto>>(order);
            return CustomResponseDto<IEnumerable<OrderDto>>.Success(200, orderDto);
        }

        public async Task<CustomResponseDto<OrderDto>> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                var errors = new List<string>();
                errors.Add("Id'ye ait  ürün bulunamadı.");
                return CustomResponseDto<OrderDto>.Fail(404, errors);
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return CustomResponseDto<OrderDto>.Success(200, orderDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> Remove(OrderDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);
            var isExistEntity = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (isExistEntity == null)
            {
                var errors = new List<string>();
                errors.Add("Id'ye ait silinecek ürün bulunamadı.");
                return CustomResponseDto<NoContentDto>.Fail(404, errors);
            }
            _orderRepository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);

        }

        public async Task<CustomResponseDto<NoContentDto>> Update(OrderDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);
            var isExistEntity = await _orderRepository.GetByIdAsync(order.Id);
            if (isExistEntity == null)
            {
                var errors = new List<string>();
                errors.Add("Id'ye ait güncellenceck ürün bulunamadı.");
                return CustomResponseDto<NoContentDto>.Fail(404, errors);
            }
            isExistEntity.TrackingNo = orderDto.TrackingNo;
            isExistEntity.Status = orderDto.Status;
            isExistEntity.UpdatedDate = DateTime.Now;

            _orderRepository.Update(isExistEntity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}
