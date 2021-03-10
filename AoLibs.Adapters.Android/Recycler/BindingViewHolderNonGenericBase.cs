using System;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace AoLibs.Adapters.Android.Recycler
{
    /// <summary>
    /// Base nongeneric class for <see cref="BindingViewHolderBase{T}"/>. Utility class used by the library.
    /// </summary>
    public abstract class BindingViewHolderNonGenericBase : RecyclerView.ViewHolder
    {
        public BindingViewHolderNonGenericBase(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public BindingViewHolderNonGenericBase(View itemView) 
            : base(itemView)
        {
        }

        public abstract void DetachBindings();
    }
}