using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Sample.Shared.Models;

namespace AoLibs.Sample.Shared.BL
{
    public class AppVariables : AppVariablesBase
    {
        public AppVariables(ISettingsProvider settingsProvider, IDataCache dataCache = null) 
            : base(settingsProvider, dataCache)
        {
        }

        public AppVariables(ISyncStorage syncStorage, IAsyncStorage asyncStorage = null) 
            : base(syncStorage, asyncStorage)
        {
        }

        [Variable]
        public Holder<UserResponse> UserResponse { get; set; }
    }
}
