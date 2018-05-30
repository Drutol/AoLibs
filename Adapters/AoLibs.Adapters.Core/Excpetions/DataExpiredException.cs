using System;

namespace AoLibs.Adapters.Core.Excpetions
{
    public class DataExpiredException : Exception
    {
        public DataExpiredException(string message) : base(message)
        {
            
        }
    }
}
