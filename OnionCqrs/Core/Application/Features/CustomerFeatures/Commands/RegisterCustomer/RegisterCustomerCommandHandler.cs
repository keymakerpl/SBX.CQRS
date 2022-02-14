using CSharpFunctionalExtensions;
using Domain.Utils;
using Domain.Utils.Customers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Commands
{
    internal class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Result<CustomerDto>>
    {
        private readonly ICustomerRepository customerRepository;

        public RegisterCustomerCommandHandler(ICustomerRepository customerRepository) =>
            this.customerRepository = customerRepository;

        public async Task<Result<CustomerDto>> Handle(RegisterCustomerCommand request,
                                                CancellationToken cancellationToken)
        {
            var firstName = PersonName.Create(request.FirstName);
            var lastName = PersonName.Create(request.LastName);
            var email = Email.Create(request.Email);

            return await Result.Combine(firstName, lastName, email)
                               .Map(() => new Customer(firstName.Value, lastName.Value, email.Value))
                               .Bind(async customer => await customerRepository.AddAsync(customer))
                               .Map(customer => new CustomerDto
                               {
                                   CustomerId = customer.Id,
                                   FirstName = firstName.Value,
                                   LastName = lastName.Value,
                                   Email = email.Value
                               });
        }
    }
}
