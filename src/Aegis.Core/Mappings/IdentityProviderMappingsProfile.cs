namespace Aegis.Core.Mappings
{
	using Aegis.Models.Users;
	using Aegis.Persistence.Entities.IdentityProvider;

	using AutoMapper;

	/// <summary>
	/// Roles Mappings Profile
	/// </summary>
	/// <seealso cref="AutoMapper.Profile" />
	public class IdentityProviderMappingsProfile : AutoMapper.Profile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityProviderMappingsProfile"/> class.
		/// </summary>
		public IdentityProviderMappingsProfile()
		{

			this.CreateMap<AegisUser, UserViewModel>(MemberList.None)
				.ForMember(m => m.LockedUser, 
				v => v.MapFrom(x => x.LockoutEnd.HasValue && x.LockoutEnd.Value > DateTime.UtcNow));
		}
	}
}
