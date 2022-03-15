using Application.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Domain.Projects.Queries.GetDevelopers
{
    public class GetDevelopersQueryHandler : IQueryHandler<GetDevelopersQuery, IEnumerable<DeveloperDto>>
    {
        private readonly IConfiguration configuration;
        private const string sql = "SELECT * FROM [Projects].[vDevelopers]";

        public GetDevelopersQueryHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<DeveloperDto>> Handle(GetDevelopersQuery request, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(configuration.GetConnectionString("OnionDatabase"));
            return await connection.QueryAsync<DeveloperDto>(sql);
        }
    }
}
