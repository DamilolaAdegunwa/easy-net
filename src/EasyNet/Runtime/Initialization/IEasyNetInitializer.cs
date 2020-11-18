namespace EasyNet.Runtime.Initialization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEasyNetInitializer
    {
        /// <summary>
        /// Init EasyNet to execute the <see cref="IEasyNetInitializationJob"/>.
        /// </summary>
        /// <returns></returns>
        void Init();
    }
}
