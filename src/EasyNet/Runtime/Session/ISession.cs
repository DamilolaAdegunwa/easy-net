namespace EasyNet.Runtime.Session
{
	/// <summary>
	/// Defines some session information that can be useful for applications.
	/// </summary>
    public interface ISession
    {
	    /// <summary>
	    /// Gets current UserId or null.
	    /// It can be null if no user logged in.
	    /// </summary>
        long? UserId { get; }
    }
}
