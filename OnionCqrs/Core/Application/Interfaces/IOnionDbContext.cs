using Domain.Utils.Customers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOnionDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        Task<int> SaveChangesAsync();
    }
}
