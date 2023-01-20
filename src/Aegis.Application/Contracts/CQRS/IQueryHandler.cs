namespace Aegis.Application.Contracts.CQRS
{
	using MediatR;

	/// <summary>
	/// Query Handler Interface
	/// </summary>
	/// <typeparam name="TQuery">The type of the query.</typeparam>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <seealso cref="IRequestHandler&lt;TQuery, TResponse&gt;" />
	public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
		where TQuery : IQuery<TResponse>
	{
	}
}
