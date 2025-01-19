using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Dto_s;
using WebScraping.Core.Models;

namespace WebScraping.Service
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
        }
    }
}
