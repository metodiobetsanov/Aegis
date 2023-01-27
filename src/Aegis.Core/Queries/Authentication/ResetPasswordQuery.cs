#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Reset Password Query
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQuery&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Core.Queries.Authentication.ResetPasswordQuery&gt;" />
	[DataContract]
	public sealed record ResetPasswordQuery : IQuery<BaseResult>
	{
	}
}
