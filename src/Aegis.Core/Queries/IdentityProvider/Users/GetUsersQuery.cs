namespace Aegis.Core.Queries.IdentityProvider.Users
{
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Models.Shared;
	using Aegis.Models.Users;

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Chimera.Application.Contracts.CQRS.IQuery&lt;Chimera.Models.Application.AjaxContentResponse&lt;IList&lt;Chimera.Models.Users.UserSimpleViewModel&gt;&gt;&gt;" />
	/// <seealso cref="MediatR.IRequest&lt;Chimera.Models.Application.AjaxContentResponse&lt;System.Collections.Generic.IList&lt;Chimera.Models.Users.UserSimpleViewModel&gt;&gt;&gt;" />
	/// <seealso cref="MediatR.IBaseRequest" />
	/// <seealso cref="IEquatable&lt;Chimera.Application.Queries.Users.GetUsersQuery&gt;" />
	public sealed record GetUsersQuery : IQuery<QueryResult<IList<UserViewModel>>>
	{
	}
}
