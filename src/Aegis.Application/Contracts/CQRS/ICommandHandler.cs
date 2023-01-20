namespace Aegis.Application.Contracts.CQRS
{
	using MediatR;

	/// <summary>
	/// Command Handler Interface
	/// </summary>
	/// <typeparam name="TCommand">The type of the command.</typeparam>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <seealso cref="IRequestHandler&lt;TCommand, TResponse&gt;" />
	public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
		where TCommand : ICommand<TResponse>
	{
	}
}
