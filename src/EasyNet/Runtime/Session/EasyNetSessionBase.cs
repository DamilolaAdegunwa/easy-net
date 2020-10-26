namespace EasyNet.Runtime.Session
{
	public abstract class EasyNetSessionBase : IEasyNetSession
	{
		/// <inheritdoc/>
		public abstract string UserId { get; }

		/// <inheritdoc/>
		public abstract string UserName { get; }

		/// <inheritdoc/>
		public abstract string Role { get; }
	}
}
