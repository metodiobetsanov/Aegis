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
	/// Command Interface
	/// </summary>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <seealso cref="IRequest&lt;TResponse&gt;" />
	public interface ICommand<out TResponse> : IRequest<TResponse>
	{
	}
}
