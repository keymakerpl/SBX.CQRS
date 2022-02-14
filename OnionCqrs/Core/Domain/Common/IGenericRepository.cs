using CSharpFunctionalExtensions;

namespace Domain.Common
{
    public interface IGenericRepository<TEntity>
        where TEntity : AggregateRoot
    {
        public Maybe<TEntity> GetById(object id);
        public Result Save(TEntity aggregateRoot);
    }
}
