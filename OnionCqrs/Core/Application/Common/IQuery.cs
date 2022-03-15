using MediatR;

namespace Application.Common
{
    public interface IQuery<out TResult> : IRequest<TResult> { }
}
