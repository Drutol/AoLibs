using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;

namespace AoLibs.Adapters.Android.Recycler
{
    public abstract class BindingViewHolderNonGenericBase : RecyclerView.ViewHolder
    {
        public BindingViewHolderNonGenericBase(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public BindingViewHolderNonGenericBase(View itemView) : base(itemView)
        {
        }

        public abstract void DetachBindings();
    }
}