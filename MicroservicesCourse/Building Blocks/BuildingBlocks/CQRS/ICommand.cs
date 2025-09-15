using MediatR;

//Used for WRITE operations
namespace BuildingBlocks.CQRS
{
    public interface ICommand : ICommand<Unit> //Unit represents an empty or no generic type in MediatR
    {

    }

    /// <summary>
    /// Interface used for write operations in CQRS
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {

    }
}
