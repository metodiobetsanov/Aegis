#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Contracts.CQRS
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
