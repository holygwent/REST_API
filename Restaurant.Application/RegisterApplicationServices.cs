using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Services;
using Restaurant.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application
{
    public static class RegisterApplicationServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();

        }
    }
}
