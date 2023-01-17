namespace Aegis.UnitTests
{
	using System;
	using System.Text;

	using Duende.IdentityServer.EntityFramework.DbContexts;
	using Duende.IdentityServer.EntityFramework.Storage;

	using global::Aegis.Application.Contracts;
	using global::Aegis.Persistence;
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

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
	}
}
