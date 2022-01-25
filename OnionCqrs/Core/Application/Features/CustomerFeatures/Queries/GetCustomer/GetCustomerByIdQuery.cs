using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Features.CustomerFeatures.Queries.GetCustomer
{
    public sealed class GetCustomerByIdQuery : IRequest<Maybe<CustomerDetailsDto>>
    {
        public GetCustomerByIdQuery(int customerId) => CustomerId = customerId;

        public int CustomerId { get; }
    }
}
