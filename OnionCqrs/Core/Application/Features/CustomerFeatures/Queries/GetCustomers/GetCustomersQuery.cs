using MediatR;
using System.Collections.Generic;

namespace Application.Features.CustomerFeatures.Queries.GetCustomers
{
    public sealed partial class GetCustomersQuery : IRequest<IEnumerable<CustomerDetailsDto>>
    {
        public GetCustomersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
    }
}