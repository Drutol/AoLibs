using System;
using System.Collections.Generic;
using System.Text;

namespace AoLibs.ApiClient.Interfaces
{
    public interface IAuthorizationProvider
    {
        bool IsAuthorized { get; }

        
    }
}
