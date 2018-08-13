using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;

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