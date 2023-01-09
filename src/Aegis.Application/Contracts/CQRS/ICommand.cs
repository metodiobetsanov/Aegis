namespace Aegis.Application.Contracts.CQRS
{
    using MediatR;

    /// <summary>
    /// Command Interface
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="IRequest&lt;TResponse&gt;" />
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
