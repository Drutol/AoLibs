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

namespace NavigationLib.Android.Navigation
{
    public abstract class ChildFragmentBase<TViewModel> : FragmentBase<TViewModel> where TViewModel : class
    {
        public ChildFragmentBase(bool hasNonTrackableBindings = false) : base(hasNonTrackableBindings)
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            NavigatedTo();

            return view;
        }
    }
}