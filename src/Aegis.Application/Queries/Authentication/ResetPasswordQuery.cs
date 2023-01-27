namespace Aegis.Application.Queries.Authentication
{
	using System.Runtime.Serialization;

	using Aegis.Application.Contracts.CQRS;
	using Aegis.Models.Shared;

	/// <summary>
	/// Reset Password Query
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQuery&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Application.Queries.Authentication.ResetPasswordQuery&gt;" />
	[DataContract]
	public sealed record ResetPasswordQuery : IQuery<BaseResult>
	{
	}
}
