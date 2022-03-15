using Domain.Projects;
using Infrastructure.Persistence.Domain.Projects;
using Infrastructure.Persistence.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
                                                        IConfiguration configuration)
        {
            SessionFactory.Initialize(configuration.GetConnectionString("OnionDatabase"));
            return services.AddScoped<IDeveloperRepository, DeveloperRepository>()
                           .AddScoped<IProjectRepository, ProjectRepository>();
        }
    }
}
