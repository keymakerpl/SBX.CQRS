using Application.Utils.Connection;
using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Queries.GetCustomers
{
    public sealed partial class GetCustomersQuery
    {
        internal sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDetailsDto>>
        {
            private readonly ISqlQueriesConnectionFactory connectionFactory;
            private const string Sql = "SELECT [CustomerId], [FirstName], [LastName], [Email] " +
                                       "FROM [Persons].[CustomersView] " +
                                       "ORDER BY CustomerId " +
                                       "OFFSET @PageSize * (@PageNumber - 1) ROWS " +
                                       "FETCH NEXT @PageSize ROWS ONLY;";

            public GetCustomersQueryHandler(ISqlQueriesConnectionFactory connectionFactory) =>
                this.connectionFactory = connectionFactory;

            public async Task<IEnumerable<CustomerDetailsDto>> Handle(
                GetCustomersQuery request,
                CancellationToken cancellationToken) =>
                await Disposable.Of(() => connectionFactory.CreateOpenConnection())
                                .Use(async connection =>
                                     await Task.FromResult(
                                         connection.Query<CustomerDetailsDto>(
                                             sql: Sql,
                                             param: new { request.PageNumber, request.PageSize })));
        }
    }
}
