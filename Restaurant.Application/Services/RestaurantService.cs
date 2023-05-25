using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        PageResult<RestaurantDto> GetAll(RestaurantQueryDto query);
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
        public PageResult<RestaurantDto> GetAll(RestaurantQueryDto query)
        {
            var baseQuery = _context.Restaurants
                .Include(x => x.Address)
                .Include(d => d.Dishes)
                .Where(x => query.searchingPhrase == null || (x.Name.ToLower().Contains(query.searchingPhrase.ToLower())
                || x.Description.ToLower().Contains(query.searchingPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.sortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant.Domain.ORM.Restaurant, object>>>
                {
                    { nameof(Domain.ORM.Restaurant.Name),r=>r.Name},
                    { nameof(Domain.ORM.Restaurant.Description),r=>r.Description},
                    { nameof(Domain.ORM.Restaurant.Category),r=>r.Category},
                };
                var selectedColumn = columnsSelectors[query.sortBy];
                baseQuery = query.sortDirection == Models.SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }
            
            var restaurants = baseQuery
                .Skip((query.pageNumber - 1) * query.pageSize)
                .Take(query.pageSize)
                .ToList();

            var totalCount = baseQuery.Count();
            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            var result = new PageResult<RestaurantDto>(restaurantsDto, totalCount, query.pageSize, query.pageNumber);
            return result;
        }
        public int Create(CreateRestaurantDto dto)
        {

            var restaurant = _mapper.Map<Restaurant.Domain.ORM.Restaurant>(dto);
            _context.Add(restaurant);
            _context.SaveChanges();
            _logger.LogWarning($"Restaurant with id:{restaurant.Id} INSERT action invoked");
            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id:{id} DELETE action invoked");
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
            _logger.LogWarning($"Restaurant with id:{id} UPDATE action invoked");
            restaurant.Name = dto.Name;
            restaurant.HasDelivery = dto.HasDelivery;
            restaurant.Description = dto.Description;
            _context.Update(restaurant);
            _context.SaveChanges();

        }
    }
}
