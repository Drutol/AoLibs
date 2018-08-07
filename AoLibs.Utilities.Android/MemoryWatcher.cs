using System;
using System.Threading;
using Android.App;
using Android.Content;
using Java.Lang;
using Debug = System.Diagnostics.Debug;

namespace AoLibs.Utilities.Android
{
    /// <summary>
    /// Utility class that listens to current memory usage of the app. So you can react early to any memory shortages.
    /// </summary>
    public class MemoryWatcher
    {
        private Timer _memoryCheckTimer;

        public long TotalRam { get; }
        public bool DisplayDebugMessages { get; } = true;
        public float MemoryWarningThresholdInPercent { get; } = 7.5f;

        /// <summary>
        /// Called whenever free percentage of memory falls below <see cref="MemoryWarningThresholdInPercent"/>
        /// </summary>
        public event EventHandler<float> MemoryWarning;

        #region Singleton

        private MemoryWatcher()
        {
            var activityManager = Application.Context.GetSystemService(Context.ActivityService) as ActivityManager;
            var memoryInfo = new ActivityManager.MemoryInfo();
            activityManager.GetMemoryInfo(memoryInfo);

            TotalRam = memoryInfo.TotalMem / (1024 * 1024);
        }

        /// <summary>
        /// Obtains and initializes new <see cref="MemoryWatcher"/>
        /// </summary>
        public static MemoryWatcher Watcher { get; } = new MemoryWatcher();

        #endregion

        private void CreateTimer(TimeSpan dueTime)
        {
            _memoryCheckTimer = new Timer(OnMemoryCheck, null, dueTime, TimeSpan.FromSeconds(15));
        }

        private void OnMemoryCheck(object state)
        {

            var available = Runtime.GetRuntime().MaxMemory();
            var used = Runtime.GetRuntime().TotalMemory();

            float percentAvailable = 100f * (1f - ((float)used / available));
            if(DisplayDebugMessages)
                Debug.WriteLine($">>>> MEMORY {used}/{available}  percent free: ({percentAvailable}%) <<<<");
            if (percentAvailable <= MemoryWarningThresholdInPercent)
            {
                MemoryWarning?.Invoke(this,percentAvailable);
            }
        }

        public void Pause()
        {
            _memoryCheckTimer?.Dispose();
            _memoryCheckTimer = null;
        }

        public void Resume(TimeSpan? dueTime)
        {
            if(_memoryCheckTimer == null)
                CreateTimer(dueTime ?? TimeSpan.Zero);
        }
    }
}