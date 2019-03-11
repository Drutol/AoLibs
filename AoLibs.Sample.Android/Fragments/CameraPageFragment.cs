using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AoLibs.Camera.Android;
using AoLibs.Camera.Android.Views;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.ViewModels;

namespace AoLibs.Sample.Android.Fragments
{
    [NavigationPage((int)PageIndex.PageCamera, NavigationPageAttribute.PageProvider.Cached)]
    public class CameraPageFragment : FragmentBase<CameraViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.camera_page;
        private CameraController5000 _controller;

        protected override void InitBindings()
        {
            _controller = new CameraController5000(Activity, AutoFitTexture);
            _controller.OnResume();
        }

        public override void OnPause()
        {
            _controller?.OnPause();
            base.OnPause();
        }

        public override void OnResume()
        {
            _controller?.OnResume();
            base.OnResume();
        }

        #region Views

        private AutoFitTextureView _autoFitTexture;
        private FaceRectView _faceRect;

        public AutoFitTextureView AutoFitTexture => _autoFitTexture ?? (_autoFitTexture = FindViewById<AutoFitTextureView>(Resource.Id.AutoFitTexture));

        //public FaceRectView FaceRect => _faceRect ?? (_faceRect = FindViewById<FaceRectView>(Resource.Id.FaceRect));

        #endregion
    }
}