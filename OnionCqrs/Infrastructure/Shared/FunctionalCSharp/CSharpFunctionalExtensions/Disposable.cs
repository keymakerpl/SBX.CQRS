using System;
using System.Threading.Tasks;

namespace CSharpFunctionalExtensions
{
    public class Disposable<T>
    where T : IDisposable
    {
        private Func<T> Factory { get; }

        internal Disposable(Func<T> factory) => this.Factory = factory;

        public void Use(Action<T> action)
        {
            using T target = this.Factory();
            action(target);
        }

        public async Task Use(Task<Action<T>> action)
        {
            using T target = this.Factory();
            await action.DefaultAwait();
        }

        public TResult Use<TResult>(Func<T, TResult> map)
        {
            using T target = this.Factory();
            return map(target);
        }

        public async Task<TResult> Use<TResult>(Func<T, Task<TResult>> map)
        {
            using T target = this.Factory();
            return await map(target).DefaultAwait();
        }
    }

    public static class Disposable
    {
        public static Disposable<T> Of<T>(Func<T> factory)
          where T : IDisposable
          => new(factory);
    }
}