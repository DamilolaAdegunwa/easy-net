namespace EasyNet.Runtime.Session
{
	/// <summary>
	/// Implements null object pattern for <see cref="IEasyNetSession"/>.
	/// </summary>
	public class NullEasyNetSession : EasyNetSessionBase
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		public static NullEasyNetSession Instance { get; } = new NullEasyNetSession();

		/// <inheritdoc/>
		public override string UserId => string.Empty;

		/// <inheritdoc/>
		public override string UserName => string.Empty;

		/// <inheritdoc/>
		public override string Role => string.Empty;
	}
}
