using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Context;
using Domain.Persons.Customers;
using Infrastructure.Persistence.Domain.Customers;

namespace Infrastructure.Persistence.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration) => 
            services.AddDbContext<OnionDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CommandsConnection"),
                                 b => b.MigrationsAssembly(typeof(OnionDbContext).Assembly.FullName)))
            .AddScoped<IOnionDbContext>(provider => provider.GetService<OnionDbContext>())
            .AddScoped<ICustomerRepository, CustomerRepository>();
    }
}
