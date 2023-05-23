using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Application.DTO;
using Restaurant.Domain.ORM;
using Restaurant.Infrastructure;
using Restaurant.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Services
{
    public interface IDishService
    {
        int Create(int id, CreateDishDto dto);
        void Delete(int restaurantId,int dishId);
        void DeleteAll(int restaurantId);
        List<DishDto> GetAll(int restaurantId);
        DishDto Get(int restaurantId, int dishId);
    }
    public class DishService: IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DishService> _logger;

        public DishService(RestaurantDbContext restaurantDbContext,IMapper mapper, ILogger<DishService> logger)
        {
            _context = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(int restaurantId , CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            _logger.LogWarning($"Dish with id:{dish.Id} INSERT action invoked");
            return dish.Id;

        }

        public void Delete(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(x=>x.Id== dishId);
            if (dish is null)
                throw new NotFoundException($"Dish with id:{dishId} not found");
            _context.Dishes.Remove(dish);
            _context.SaveChanges();
            _logger.LogWarning($"Dish with id:{dishId} DELETE action invoked");
        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            _context.Dishes.RemoveRange(restaurant.Dishes);
            _context.SaveChanges();
            _logger.LogWarning($"restaurant with id:{restaurantId} DELETE All dishes action invoked");
        }

        public DishDto Get(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == dishId);
            if (dish is null)
                throw new NotFoundException($"Dish with id:{dishId} not found");
            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dishes = restaurant.Dishes.ToList();
            var dishesDto = _mapper.Map<List<DishDto>>(dishes);
            return dishesDto ;
        }

        private Restaurant.Domain.ORM.Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _context.Restaurants
               .Include(x => x.Dishes)
               .FirstOrDefault(x => x.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException($"Restaurant with id {restaurantId} not found");
            return restaurant;
        }
    }
}
