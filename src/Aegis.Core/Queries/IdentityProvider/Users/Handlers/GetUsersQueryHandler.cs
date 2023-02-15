namespace Aegis.Core.Queries.IdentityProvider.Users.Handlers
{
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Queries.IdentityProvider.Users;
	using Aegis.Models.Shared;
	using Aegis.Models.Users;
	using Aegis.Persistence.Entities.IdentityProvider;

	using AutoMapper;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// GetUsers Query Handler
	/// </summary>
	/// <seealso cref="IQueryHandler&lt;GetUsersQuery, QueryResult&lt;IList&lt;UserViewModel&gt;&gt;&gt;" />
	public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, QueryResult<IList<UserViewModel>>>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<GetUsersQueryHandler> _logger;

		/// <summary>
		/// The mapper
		/// </summary>
		private readonly IMapper _mapper;

		/// <summary>
		/// The user in manager.
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		/// <param name="userManager">The user manager.</param>
		public GetUsersQueryHandler(
			ILogger<GetUsersQueryHandler> logger,
			IMapper mapper,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_mapper = mapper;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><see cref=" QueryResult&lt;IList&lt;UserViewModel&gt;&gt;&gt;"/></returns>
		public Task<QueryResult<IList<UserViewModel>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(GetUsersQuery));
			QueryResult<IList<UserViewModel>> result = QueryResult<IList<UserViewModel>>.Failed();

			try
			{
				List<UserViewModel> users = _userManager.Users.Select(x => _mapper.Map<UserViewModel>(x)).ToList();

				result = QueryResult<IList<UserViewModel>>.Succeeded(users);
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SendForgetPasswordCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrong, ex.Message, ex);
			}

			return Task.FromResult(result);
		}
	}
}
