using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.SwipeRefreshLayout.Widget;

namespace AoLibs.Utilities.Android.Views
{
    public class ScrollableSwipeToRefreshLayout : SwipeRefreshLayout
    {
        public ScrollableSwipeToRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ScrollableSwipeToRefreshLayout(Context context)
            : base(context)
        {
        }

        public ScrollableSwipeToRefreshLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public View ScrollingView { get; set; }
        public bool CanRefresh { get; set; }

        public override bool CanChildScrollUp()
        {
            if (ScrollingView != null)
                return ScrollingView.CanScrollVertically(-1);
            return !CanRefresh;
        }
    }
}