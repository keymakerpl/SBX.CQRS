using CSharpFunctionalExtensions;
using Domain.Persons.Customers;
using Infrastructure.Persistence.Context;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Domain.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OnionDbContext dbContext;

        public CustomerRepository(OnionDbContext dbContext) => this.dbContext = dbContext;

        public async Task<Result<Customer>> AddAsync(Customer customer) =>
            await Result.Try(async () => await dbContext.AddAsync(customer))
                        .Ensure(async _ => await dbContext.SaveChangesAsync() > 0, "Cannot write entity to database")
                        .Map(entry => entry.Entity);

        public async Task<Maybe<Customer>> GetByIdAsync(CustomerId id) =>
            Maybe.From(await dbContext.FindAsync<Customer>(id.Value));
    }
}
