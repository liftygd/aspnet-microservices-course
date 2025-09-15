using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQuery : IQuery<Unit>
    {

    }

    /// <summary>
    /// Interface used for read opeartion in CQRS.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {

    }
}
