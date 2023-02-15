#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Areas.Admin.Controllers
{
	using Aegis.Core.Constants;
	using Aegis.Core.Queries.IdentityProvider.Users;
	using Aegis.Models.Shared;
	using Aegis.Models.Users;

	using MediatR;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using NuGet.Packaging;

	/// <summary>
	/// Users Controller
	/// </summary>
	/// <seealso cref="Controller" />
	[Authorize]
	[Area(ApplicationConstants.AdminArea)]
	public partial class UsersController : Controller
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<UsersController> _logger;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		public UsersController(ILogger<UsersController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		/// <summary>
		/// Users index.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Index()
			=> this.View();
	}
}
