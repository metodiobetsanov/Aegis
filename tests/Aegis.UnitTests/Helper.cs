﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests
{
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Options;

	public static class Helper
	{
		/// <summary>
		/// Converts to base64.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string ToBase64(this string data)
			=> Convert.ToBase64String(Encoding.UTF8.GetBytes(data));

		/// <summary>
		/// Gets the service collection.
		/// </summary>
		/// <returns></returns>
		public static ServiceCollection GetServiceCollection()
		{
			ServiceCollection services = new ServiceCollection();

			services.AddScoped<IDataProtectionProvider>(x => new Mock<IDataProtectionProvider>().Object);
			services.AddSingleton<IPersonalDataProtector>(x => new Mock<IPersonalDataProtector>().Object);
			services.AddSingleton<ILookupProtector>(x => new Mock<ILookupProtector>().Object);
			services.AddSingleton<ILookupProtectorKeyRing>(x => new Mock<ILookupProtectorKeyRing>().Object);
			return services;
		}

		/// <summary>
		/// Gets the service scope factory.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns></returns>
		public static Mock<IServiceScopeFactory> GetServiceScopeFactoryMock(ServiceCollection services)
		{
			Mock<IServiceScopeFactory> scf = new Mock<IServiceScopeFactory>();
			Mock<IServiceScope> sc = new Mock<IServiceScope>();

			sc.Setup(x => x.ServiceProvider)
				.Returns(services.BuildServiceProvider());

			scf.Setup(x => x.CreateScope())
				.Returns(sc.Object);

			return scf;
		}

		/// <summary>
		/// Gets the user manager mock.
		/// </summary>
		/// <returns></returns>
		public static Mock<UserManager<AegisUser>> GetUserManagerMock()
		{
			Mock<IUserStore<AegisUser>> store = new Mock<IUserStore<AegisUser>>();
			Mock<UserManager<AegisUser>> mgr = new Mock<UserManager<AegisUser>>(store.Object, null, null, null, null, null, null, null, null);
			mgr.Object.UserValidators.Add(new UserValidator<AegisUser>());
			mgr.Object.PasswordValidators.Add(new PasswordValidator<AegisUser>());

			return mgr;
		}

		/// <summary>
		/// Gets the role manager mock.
		/// </summary>
		/// <returns></returns>
		public static Mock<RoleManager<AegisRole>> GetRoleManagerMock()
		{
			Mock<IRoleStore<AegisRole>> store = new Mock<IRoleStore<AegisRole>>();
			Mock<RoleManager<AegisRole>> mgr = new Mock<RoleManager<AegisRole>>(store.Object, null, null, null, null);
			mgr.Object.RoleValidators.Add(new RoleValidator<AegisRole>());

			return mgr;
		}

		/// <summary>
		/// Gets the user manager mock.
		/// </summary>
		/// <returns></returns>
		public static Mock<SignInManager<AegisUser>> GetSignInManagerMock()
		{
			Mock<SignInManager<AegisUser>> mgr = new Mock<SignInManager<AegisUser>>(
				GetUserManagerMock().Object,
				new Mock<IHttpContextAccessor>().Object,
				new Mock<IUserClaimsPrincipalFactory<AegisUser>>().Object,
				new Mock<IOptions<IdentityOptions>>().Object,
				new Mock<ILogger<SignInManager<AegisUser>>>().Object,
				new Mock<IAuthenticationSchemeProvider>().Object,
				new Mock<IUserConfirmation<AegisUser>>().Object);

			return mgr;
		}

		/// <summary>
		/// Gets the user faker.
		/// </summary>
		/// <returns></returns>
		public static Faker<AegisUser> GetUserFaker()
			=> new Faker<AegisUser>("en")
			.RuleFor(r => r.Id, f => f.Random.Guid())
			.RuleFor(r => r.Email, f => f.Internet.Email())
			.RuleFor(r => r.EmailConfirmed, f => f.Random.Bool())
			.RuleFor(r => r.PhoneNumber, f => f.Phone.PhoneNumber())
			.RuleFor(r => r.PhoneNumberConfirmed, f => f.Random.Bool())
			.RuleFor(r => r.FirstName, f => f.Name.FirstName())
			.RuleFor(r => r.LastName, f => f.Name.LastName())
			.RuleFor(r => r.ProfilePictureURL, f => f.Image.LoremFlickrUrl())
			.RuleFor(r => r.SystemUser, f => f.Random.Bool())
			.RuleFor(r => r.ProtectedUser, f => f.Random.Bool())
			.RuleFor(r => r.ActiveProfile, f => f.Random.Bool())
			.RuleFor(r => r.CompletedProfile, f => f.Random.Bool())
			.RuleFor(r => r.SoftDelete, f => f.Random.Bool())
			.RuleFor(r => r.CreatedOn, f => f.Date.Past(1))
			.RuleFor(r => r.UpdatedOn, f => f.Date.Past(1));

		/// <summary>
		/// Gets the role faker.
		/// </summary>
		/// <returns></returns>
		public static Faker<AegisRole> GetRoleFaker()
			=> new Faker<AegisRole>("en")
			.RuleFor(r => r.Id, f => f.Random.Guid())
			.RuleFor(r => r.Name, f => f.Name.FirstName())
			.RuleFor(r => r.Description, f => f.Lorem.Sentence())
			.RuleFor(r => r.SystemRole, f => f.Random.Bool())
			.RuleFor(r => r.ProtectedRole, f => f.Random.Bool())
			.RuleFor(r => r.AssignByDefault, f => f.Random.Bool())
			.RuleFor(r => r.CreatedOn, f => f.Date.Past(1))
			.RuleFor(r => r.UpdatedOn, f => f.Date.Past(1));
	}
}
