using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Domain.Persons.Customers
{
    public interface ICustomerRepository
    {
        Task<Maybe<Customer>> GetByIdAsync(CustomerId id);
        Task<Result<Customer>> AddAsync(Customer customer);
    }
}
