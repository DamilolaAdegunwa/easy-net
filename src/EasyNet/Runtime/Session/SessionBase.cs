namespace EasyNet.Runtime.Session
{
    public abstract class SessionBase : ISession
    {
        /// <inheritdoc/>
        public abstract long? UserId { get; }
        
        protected SessionBase()
        {
        }
    }
}
