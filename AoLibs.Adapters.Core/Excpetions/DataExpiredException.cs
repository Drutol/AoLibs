using System;

namespace AoLibs.Adapters.Core.Excpetions
{
    /// <summary>
    /// Exception that signifies that the data stored is older than specified maximum.
    /// </summary>
    public class DataExpiredException : Exception
    {
        public DataExpiredException(string message) 
            : base(message)
        {          
        }
    }
}
