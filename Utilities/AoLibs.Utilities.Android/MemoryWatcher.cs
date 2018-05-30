using System;
using System.Threading;
using Android.App;
using Android.Content;
using Java.Lang;
using Debug = System.Diagnostics.Debug;

namespace AoLibs.Utilities.Android
{
    public class MemoryWatcher
    {
        public long TotalRam { get; set; }
        public bool DisplayDebugMessages { get; set; } = true;
        public float MemoryWarningThresholdInPercent { get; set; } = 7.5f;
        public event EventHandler<float> MemoryWarning;


        private Timer _memoryCheckTimer;

        #region Singleton

        private MemoryWatcher()
        {
            var activityManager = Application.Context.GetSystemService(Context.ActivityService) as ActivityManager;
            var memoryInfo = new ActivityManager.MemoryInfo();
            activityManager.GetMemoryInfo(memoryInfo);

            TotalRam = memoryInfo.TotalMem / (1024 * 1024);
        }

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
                Runtime.GetRuntime().Gc();
                Pause();
                Resume(TimeSpan.FromSeconds(10));
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