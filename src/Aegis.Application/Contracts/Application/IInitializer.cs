namespace Aegis.Application.Contracts.Application
{
	/// <summary>
	/// Initializer interface
	/// </summary>
	public interface IInitializer
	{
		/// <summary>
		/// Run the initializer.
		/// </summary>
		/// <returns></returns>
		Task Initialize();
	}
}
