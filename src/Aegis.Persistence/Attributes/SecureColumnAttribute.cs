namespace Aegis.Persistence.Attributes
{
	using System;

	/// <summary>
	/// Secure Column Attribute
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class SecureColumnAttribute : Attribute
	{
	}
}
