using System;
using Android.Content;
using Android.Content.Res;
using Android.Hardware;
using Android.Views;

namespace AoLibs.Camera.Android.Utils
{
    public class SimpleOrientationListener : OrientationEventListener
    {
        private Orientation _defaultScreenOrientation = Orientation.Undefined;

        private int _prevOrientation = (int)Orientation.Undefined;

        public event EventHandler<Orientation> OrientationChanged;
        public event EventHandler<SurfaceOrientation> SurfaceOrientationChanged;

        public SurfaceOrientation CurrentSurfaceOrientation { get; set; }
        public Orientation CurrentOrientation { get; set; }

        public static SimpleOrientationListener Instance { get; private set; }

        public static void Init(Context context, SensorDelay rate)
        {
            Instance = new SimpleOrientationListener(context, rate);
            Instance.Enable();
        }

        private SimpleOrientationListener(Context context) : base(context)
        {

        }

        private SimpleOrientationListener(Context context, SensorDelay rate) : base(context, rate)
        {

        }

        public override void OnOrientationChanged(int orientation)
        {
            int currentOrientation = (int)SurfaceOrientation.Rotation0;
            if (orientation >= 330 || orientation < 30)
            {
                currentOrientation = (int)SurfaceOrientation.Rotation0;
            }
            else if (orientation >= 60 && orientation < 120)
            {
                currentOrientation = (int)SurfaceOrientation.Rotation90;
            }
            else if (orientation >= 150 && orientation < 210)
            {
                currentOrientation = (int)SurfaceOrientation.Rotation180;
            }
            else if (orientation >= 240 && orientation < 300)
            {
                currentOrientation = (int)SurfaceOrientation.Rotation270;
            }

            if (_prevOrientation != currentOrientation && orientation != OrientationEventListener.OrientationUnknown)
            {
                _prevOrientation = currentOrientation;
                if (currentOrientation != OrientationEventListener.OrientationUnknown)
                    ReportOrientationChanged((SurfaceOrientation)currentOrientation);
            }

        }

        private void ReportOrientationChanged(SurfaceOrientation currentOrientation)
        {
            Orientation toReportOrientation;

            if (currentOrientation == SurfaceOrientation.Rotation0 || currentOrientation == SurfaceOrientation.Rotation180)
                toReportOrientation = Orientation.Portrait;
            else
                toReportOrientation = Orientation.Landscape;

            CurrentSurfaceOrientation = currentOrientation;
            CurrentOrientation = toReportOrientation;
            OrientationChanged?.Invoke(this, toReportOrientation);
            SurfaceOrientationChanged?.Invoke(this, currentOrientation);
        }
    }
}
