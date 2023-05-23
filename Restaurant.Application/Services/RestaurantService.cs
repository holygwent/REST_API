using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant.Application.DTO;
using Restaurant.Infrastructure;
using Restaurant.Infrastructure.Exceptions;

namespace Restaurant.Application.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto dto);
        void Delete(int id);
        void Update(UpdateRestaurantDto dto, int id);

    }
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext restaurantDbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _context = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
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
            _logger.LogWarning($"Restaurant with id:{restaurant.Id} Add action invoked");
            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id:{id} Delete action invoked");
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant is null) 
                throw new NotFoundException($"there is no restaurant with id {id}");
            _context.Remove(restaurant);
            _context.SaveChanges();


        }
        public void Update(UpdateRestaurantDto dto, int id)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant == null)
                throw new NotFoundException($"there is no restaurant with id {id}");
            _logger.LogWarning($"Restaurant with id:{id} Update action invoked");
            restaurant.Name = dto.Name;
            restaurant.HasDelivery = dto.HasDelivery;
            restaurant.Description = dto.Description;
            _context.Update(restaurant);
            _context.SaveChanges();
      
        }
    }
}
