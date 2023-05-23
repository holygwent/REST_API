using AutoMapper;
using Restaurant.Application.DTO;
using Restaurant.Domain.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Restaurant.Application.Mapper
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant.Domain.ORM.Restaurant, RestaurantDto>()
                .ForMember(x => x.City, c => c.MapFrom(d => d.Address.City))
                .ForMember(x => x.Street, c => c.MapFrom(d => d.Address.Street))
                .ForMember(x => x.PostalCode, p => p.MapFrom(d => d.Address.PostaCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant.Domain.ORM.Restaurant>()
                .ForMember(x => x.Address, c => c.MapFrom(dto => new Address { City = dto.City, Street = dto.Street, PostaCode = dto.PostalCode }));

        }
    }
}
