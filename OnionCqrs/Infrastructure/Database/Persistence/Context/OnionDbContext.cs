using Application.Interfaces;
using Domain.Utils;
using Domain.Utils.Customers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context
{
    public class OnionDbContext : DbContext, IOnionDbContext
    {
        public OnionDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnionDbContext).Assembly);
    }
}
