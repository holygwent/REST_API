using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTO;
using Restaurant.Infrastructure;

namespace Restaurant.Application.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto);
        bool Delete(int id);
        bool Update(UpdateRestaurantDto dto,int id);

    }
    public class RestaurantService: IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public RestaurantService(RestaurantDbContext restaurantDbContext,IMapper mapper)
        {
            _context = restaurantDbContext;
            _mapper = mapper;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _context.Restaurants
                   .Include(x => x.Address)
                   .Include(d => d.Dishes)
                   .FirstOrDefault(x => x.Id == id);

            if (restaurant == null)
                return null;

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;

        }
        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _context.Restaurants
                .Include(x => x.Address)
                .Include(d => d.Dishes)
                .ToList();
            var result = _mapper.Map<List<RestaurantDto>>(restaurants);
            return result;
        }
        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant.Domain.ORM.Restaurant>(dto);
            _context.Add(restaurant);
            _context.SaveChanges();
            return restaurant.Id;
        }

        public bool Delete(int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x=>x.Id==id);
            if(restaurant is null) return false;
            _context.Remove(restaurant);
            _context.SaveChanges();
            return true;

        }
        public bool Update(UpdateRestaurantDto dto,int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant == null)
                return false;
            restaurant.Name = dto.Name;
            restaurant.HasDelivery = dto.HasDelivery;
            restaurant.Description = dto.Description;
            _context.Update(restaurant);
            _context.SaveChanges();
            return true;
        }
    }
}
