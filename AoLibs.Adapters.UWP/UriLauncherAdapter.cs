using System;
using Windows.System;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Allows to launch Uri by system.
    /// </summary>
    public class UriLauncherAdapter : IUriLauncherAdapter
    {
        public async void LaunchUri(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}