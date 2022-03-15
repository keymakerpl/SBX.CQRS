using Application.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Application.Domain.Projects.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IConfiguration configuration;
        private const string sql = "SELECT * FROM [Projects].[vProjects]";

        public GetProjectsQueryHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("OnionDatabase"));
            return await connection.QueryAsync<ProjectDto>(sql);
        }
    }
}
