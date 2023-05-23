using Microsoft.Extensions.DependencyInjection;
using Restaurant.Infrastructure.Middleware;
using Restaurant.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<RestaurantDbContext>();
            services.AddScoped<RestaurantSeederService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();


        }
    }
}
