using System;
using Android.Widget;

namespace AoLibs.Utilities.Android.Listeners
{
    public class OnScrollListener : Java.Lang.Object , AbsListView.IOnScrollListener
    {
        private readonly Action<(AbsListView, int firstVisibleItem, int visibleItemCount, int totalItemCount)> _actionOnScroll;
        private readonly Action<(AbsListView, ScrollState)> _onScrollChangedAction;

        public OnScrollListener(Action<(AbsListView, int firstVisibleItem, int visibleItemCount, int totalItemCount)> actionOnScroll = null, Action<(AbsListView,ScrollState)> onScrollChangedAction = null)
        {
            _actionOnScroll = actionOnScroll;
            _onScrollChangedAction = onScrollChangedAction;
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            _actionOnScroll?.Invoke((view,  firstVisibleItem,  visibleItemCount,  totalItemCount));
        }

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            _onScrollChangedAction?.Invoke((view,scrollState));
        }
    }
}