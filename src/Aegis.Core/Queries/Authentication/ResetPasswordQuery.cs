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
