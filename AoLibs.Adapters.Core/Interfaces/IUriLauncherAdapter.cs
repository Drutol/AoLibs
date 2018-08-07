using System;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Allows to launch Uri.
    /// </summary>
    public interface IUriLauncherAdapter
    {
        /// <summary>
        /// Launches given Uri.
        /// </summary>
        /// <param name="uri"></param>
        void LaunchUri(Uri uri);
    }
}
