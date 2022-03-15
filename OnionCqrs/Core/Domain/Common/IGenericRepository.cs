using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IGenericRepository<TEntity>
        where TEntity : AggregateRoot
    {
        public Maybe<TEntity> GetById(object id);
        public Task<Maybe<TEntity>> GetByIdAsync(object id, CancellationToken cancellationToken = default);
        public Result Save(TEntity aggregateRoot);
        public Task<Result> SaveAsync(TEntity aggregateRoot, CancellationToken cancellationToken = default);
    }
}
