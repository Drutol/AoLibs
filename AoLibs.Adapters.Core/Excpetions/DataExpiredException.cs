using System;

namespace AoLibs.Adapters.Core.Excpetions
{
    /// <summary>
    /// Exception that signifies that the data stored is older than specified maximum.
    /// </summary>
    public class DataExpiredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataExpiredException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        public DataExpiredException(string message) 
            : base(message)
        {          
        }
    }
}
