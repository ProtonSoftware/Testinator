using System;

namespace Testinator.Core
{
    /// <summary>
    /// The base builder class
    /// </summary>
    /// <typeparam name="T">The type of objects this bulder creates</typeparam>
    public abstract class Builder<T> : IBuilder<T>
        where T : class
    {
        #region Protected Members

        /// <summary>
        /// The object this builder is forming
        /// </summary>
        protected T CreatedObject { get; set; }

        /// <summary>
        /// Checks if the object can be returned from this builder
        /// </summary>
        /// <returns>True if the object can be returned; otherwise, false</returns>
        protected virtual bool IsReady() => CreatedObject != null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the result of this builder work
        /// NOTE: override this method to cumstumize finalizing procedure
        /// </summary>
        /// <returns>The object tha was created using this builder</returns>
        public virtual T GetResult()
        {
            if (IsReady())
                return CreatedObject;

            return null;
        }

        #endregion
    }
}
