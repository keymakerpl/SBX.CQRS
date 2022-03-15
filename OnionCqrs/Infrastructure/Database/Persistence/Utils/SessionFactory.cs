using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Event;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Utils
{
    public static class SessionFactory
    {
        private static ISessionFactory _factory;

        public static ISession OpenSession() => _factory.OpenSession();

        public static void Initialize(string connectionString) => _factory = BuildSessionFactory(connectionString);

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(
                        ForeignKey.EndsWith("Id"),
                        ConventionBuilder.Property
                            .When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
                )
                .ExposeConfiguration(x =>
                {
                    x.EventListeners.PostCommitUpdateEventListeners =
                        new IPostUpdateEventListener[] { new EventListener() };
                    x.EventListeners.PostCommitInsertEventListeners =
                        new IPostInsertEventListener[] { new EventListener() };
                    x.EventListeners.PostCommitDeleteEventListeners =
                        new IPostDeleteEventListener[] { new EventListener() };
                    x.EventListeners.PostCollectionUpdateEventListeners =
                        new IPostCollectionUpdateEventListener[] { new EventListener() };
                });

            return configuration.BuildSessionFactory();
        }
    }

    internal class EventListener :
        IPostInsertEventListener,
        IPostDeleteEventListener,
        IPostUpdateEventListener,
        IPostCollectionUpdateEventListener
    {
        public void OnPostDelete(PostDeleteEvent @event)
        {
            
        }

        public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            
        }

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent @event)
        {
            
        }

        public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
