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
