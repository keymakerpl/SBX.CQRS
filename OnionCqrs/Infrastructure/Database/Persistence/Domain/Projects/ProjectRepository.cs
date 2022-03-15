using Domain.Projects;
using Infrastructure.Persistence.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Domain.Projects
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ILogger<ProjectRepository> logger) : base(logger)
        {
        }
    }
}
