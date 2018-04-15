namespace Testinator.Core
{
    /// <summary>
    /// Base builder interface
    /// </summary>
    /// <typeparam name="TTarget">The target items this builder creates</typeparam>
    public interface IBuilder<TTarget>
    {
        /// <summary>
        /// Gets the result of this builder's work
        /// </summary>
        /// <returns></returns>
        TTarget GetResult();
    }
}
