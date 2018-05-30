using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AoLibs.Utilities.Android.Listeners;

namespace AoLibs.Utilities.Android.Views
{
    public static class ViewExtensions
    {
        public static void SetAdapter(this LinearLayout layout, BaseAdapter adapter)
        {
            layout.RemoveAllViews();
            for (int i = 0; i < adapter.Count; i++)
            {
                layout.AddView(adapter.GetView(i, null, layout));
            }
        }

        public static void MakeFlingAware(this AbsListView list, Action<bool> action)
        {
            list.SetOnScrollListener(new OnScrollListener(null,tuple => 
            {
                action.Invoke(tuple.Item2 == ScrollState.Fling);
            }));
        }

    

        public static bool IsKeyboardVisibile(View rootView)
        {
            var r = new Rect();
            rootView.GetWindowVisibleDisplayFrame(r);
            int keypadHeight = rootView.RootView.Height - r.Bottom;

            if (keypadHeight > rootView.Height * 0.15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void HideKeyboard(View rootView)
        {
            if (IsKeyboardVisibile(rootView))
            {
                var inputManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
                inputManager.ToggleSoftInput(ShowFlags.Forced, 0);
            }
        }

        /// <summary>
        /// Sets margins. All provided values are to be provided in dp.
        /// </summary>
        public static void SetMargins(this View view, float l, float t, float r, float b)
        {
            var param = view.LayoutParameters as ViewGroup.MarginLayoutParams;
            param.SetMargins(DimensionsHelper.DpToPx(l), DimensionsHelper.DpToPx(t), DimensionsHelper.DpToPx(r),
                DimensionsHelper.DpToPx(b));
            view.LayoutParameters = param;
        }          
        
        /// <summary>
        /// Sets margins. All provided values are to be provided in dp.
        /// </summary>
        public static void SetMargins(this View view, float uniform)
        {
            SetMargins(view,uniform,uniform,uniform,uniform);
        }       
        
        /// <summary>
        /// Sets margins. All provided values are to be provided in dp.
        /// </summary>
        public static void SetMargins(this View view, float horizontal, float vertical)
        {
            SetMargins(view,horizontal,vertical,horizontal,vertical);
        }       
        
        /// <summary>
        /// Sets padding. All provided values are to be provided in dp.
        /// </summary>
        public static void SetPadding(this View view, float l, float t, float r, float b)
        {
            view.SetPadding(DimensionsHelper.DpToPx(l), DimensionsHelper.DpToPx(t), DimensionsHelper.DpToPx(r),
                DimensionsHelper.DpToPx(b));
        }
    }
}