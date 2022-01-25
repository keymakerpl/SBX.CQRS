using Application.Utils.Connection;
using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Queries.GetCustomer
{
    public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Maybe<CustomerDetailsDto>>
    {
        private readonly ISqlQueriesConnectionFactory connectionFactory;
        private const string Sql = "SELECT [CustomerId], [FirstName], [LastName], [Email] " +
                                   "FROM [dbo].[Customers] " +
                                   "WHERE CustomerId = @CustomerId ";

        public GetCustomerByIdQueryHandler(ISqlQueriesConnectionFactory connectionFactory) =>
            this.connectionFactory = connectionFactory;

        public async Task<Maybe<CustomerDetailsDto>> Handle(
            GetCustomerByIdQuery request,
            CancellationToken cancellationToken) =>
            await Disposable.Of(() => connectionFactory.CreateOpenConnection())
                            .Use(async connection =>
                                 await Task.FromResult(
                                     connection.Query<CustomerDetailsDto>(
                                         sql: Sql,
                                         param: new { request.CustomerId }).TryFirst()));
    }
}
