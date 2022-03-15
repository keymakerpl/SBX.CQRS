using CSharpFunctionalExtensions;
using Domain.Common;
using Infrastructure.Persistence.Utils;
using Microsoft.Extensions.Logging;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Common
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : AggregateRoot
    {
        private readonly ILogger logger;

        public GenericRepository(ILogger logger)
        {
            this.logger = logger;
        }

        public Maybe<T> GetById(object id)
        {
            using ISession session = SessionFactory.OpenSession();
            return session.Get<T>(id);
        }

        public async Task<Maybe<T>> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            using ISession session = SessionFactory.OpenSession();
            return await session.GetAsync<T>(id, cancellationToken: cancellationToken);
        }

        public Result Save(T aggregateRoot) =>
            Result.Try(() =>
            {
                using ISession session = SessionFactory.OpenSession();
                using ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(aggregateRoot);
                transaction.Commit();
            }, ex => ExceptionHandler.LogError(ex, logger));

        public async Task<Result> SaveAsync(T aggregateRoot, CancellationToken cancellationToken = default) =>
        await Result.Try(async () =>
            {
                using ISession session = SessionFactory.OpenSession();
                using ITransaction transaction = session.BeginTransaction();
                await session.SaveOrUpdateAsync(aggregateRoot, cancellationToken: cancellationToken);
                await transaction.CommitAsync();
            }, ex => ExceptionHandler.LogError(ex, logger));
    }
}
