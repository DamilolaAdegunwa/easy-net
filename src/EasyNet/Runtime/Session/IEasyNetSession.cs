namespace EasyNet.Runtime.Session
{
	/// <summary>
	/// Defines some session information that can be useful for applications.
	/// </summary>
	public interface IEasyNetSession
	{
		/// <summary>
		/// Gets current UserId or null.
		/// It can be empty if no user logged in.
		/// </summary>
		string UserId { get; }

		/// <summary>
		/// Gets current UserName or empty.
		/// It can be empty if no user logged in.
		/// </summary>
		string UserName { get; }

		/// <summary>
		/// Gets current UserRole or empty.
		/// It can be empty if no user logged in.
		/// </summary>
		string Role { get; }
	}
}
