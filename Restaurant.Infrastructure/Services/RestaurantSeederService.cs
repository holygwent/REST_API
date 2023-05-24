using Restaurant.Domain.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.Services
{
    public class RestaurantSeederService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly List<Restaurant.Domain.ORM.Restaurant> _restaurants = new List<Restaurant.Domain.ORM.Restaurant>()
        {
            new Restaurant.Domain.ORM.Restaurant()
            {
                Name = "KFC",
                Category = "FastFood",
                Description="KFC restaurant",
                ContactEmail="contact@kfc.com",
                ContactNumber="123456789",
                HasDelivery=true,
                Dishes = new List<Dish>()
                {
                    new Dish(){Name = "Hot Chicken",Price=10.30M,Description="spicy chiken breast"},
                    new Dish(){Name = "Chicken Nugets",Price=5.30M,Description="delicoius chicken nugets with sauce"}

                },
                Address = new Address()
                {
                    City="Kraków",
                    Street="Długa 5",
                    PostaCode="30-001"
                }
            },
            new Restaurant.Domain.ORM.Restaurant()
            {
                Name = "Macdonald",
                Category = "FastFood",
                Description="macdonald restaurant",
                ContactEmail="contact@mac.com",
                ContactNumber="123456789",
                HasDelivery=true,
                Dishes = new List<Dish>()
                {
                    new Dish(){Name = "Hamburger",Price=10.30M,Description="delicoius burger with beef"},
                    new Dish(){Name = "Cheeseburger",Price=11.30M,Description="delicoius burger with cheese and meat"}

                },
                Address = new Address()
                {
                    City="Warszawa",
                    Street="Kolorowa 5",
                    PostaCode="30-002"
                }
            }
        };
        private readonly List<Role> _roles = new List<Role>()
        {
            new Role(){Id=1,Name="User"},
            new Role(){Id=2,Name="Manager"},
            new Role(){Id=3,Name="Admin"}
        };
        public RestaurantSeederService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Seed()
        {

            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    _dbContext.Roles.AddRange(_roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    _dbContext.Restaurants.AddRange(_restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
