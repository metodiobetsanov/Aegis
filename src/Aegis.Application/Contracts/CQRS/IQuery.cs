namespace Aegis.Application.Contracts.CQRS
{
	using MediatR;

	/// <summary>
	/// Query Interface
	/// </summary>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <seealso cref="IRequest&lt;TResponse&gt;" />
	public interface IQuery<out TResponse> : IRequest<TResponse>
	{
	}
}
