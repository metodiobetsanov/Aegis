namespace Aegis.Application.Contracts.IInitializers
{
	/// <summary>
	/// Initializer interface
	/// </summary>
	public interface IInitializer
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="IInitializer"/> is initialized.
		/// </summary>
		/// <value>
		///   <c>true</c> if initialized; otherwise, <c>false</c>.
		/// </value>
		public bool Initialized { get; }

		/// <summary>
		/// Run the initializer.
		/// </summary>
		/// <returns></returns>
		Task Initialize();
	}
}
