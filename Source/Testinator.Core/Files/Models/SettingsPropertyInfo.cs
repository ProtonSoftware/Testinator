using System;

namespace Testinator.Core
{
    /// <summary>
    /// Constains informations about C# class property
    /// Our own implementation of <see cref="PropertyInfo"/> which allows to create an instance of it
    /// </summary>
    public class SettingsPropertyInfo
    {
        #region Public Properties
        
        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the property such as string/int/etc.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// The value of this property
        /// </summary>
        public object Value { get; set; }

        #endregion
    }
}
