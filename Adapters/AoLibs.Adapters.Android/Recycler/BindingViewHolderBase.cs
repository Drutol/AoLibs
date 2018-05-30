using System.Collections.Generic;
using Android.Views;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Adapters.Android.Recycler
{
    public abstract class BindingViewHolderBase<T> : BindingViewHolderNonGenericBase
    {
        private T _viewModel;
        protected View View;

        public T ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                foreach (var binding in Bindings)               
                    binding.Detach();
                Bindings.Clear();         
                SetBindings();
            }
        }

        protected List<Binding> Bindings { get; } = new List<Binding>();
   
        public BindingViewHolderBase(View view) : base(view)
        {           
            View = view;
        }

        public override void DetachBindings()
        {
            foreach (var binding in Bindings)           
                binding.Detach();           
        }

        protected abstract void SetBindings();
    }
}