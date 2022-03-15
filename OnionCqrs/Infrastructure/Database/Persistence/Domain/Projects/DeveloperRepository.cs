using Domain.Projects;
using Infrastructure.Persistence.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Domain.Projects
{
    public class DeveloperRepository : GenericRepository<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(ILogger<DeveloperRepository> logger) : base(logger)
        {
        }
    }
}
