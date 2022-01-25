using Application.Utils.Connection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services.AddMediatR(Assembly.GetExecutingAssembly())
                    .AddScoped<ISqlQueriesConnectionFactory, SqlQueriesConnectionFactory>()
                    .AddSingleton(sp => 
                        new QueriesConnectionString(sp.GetRequiredService<IConfiguration>()
                                                      .GetConnectionString("QueriesConnection")))
                    .AddSingleton(sp => 
                        new CommandsConnectionString(sp.GetRequiredService<IConfiguration>()
                                                       .GetConnectionString("CommandsConnection")));
    }
}
