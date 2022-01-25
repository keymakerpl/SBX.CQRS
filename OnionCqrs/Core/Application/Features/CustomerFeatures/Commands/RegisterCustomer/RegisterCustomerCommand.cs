using CSharpFunctionalExtensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.CustomerFeatures.Commands
{
    public class RegisterCustomerCommand : IRequest<Result<CustomerDto>>
    {
        public RegisterCustomerCommand(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
