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
using AoLibs.Adapters.Core.Dialogs;

namespace AoLibs.Adapters.Android.Dialogs
{
    public abstract class CustomArgumentViewModelDialogBase<TViewModel,TArgument> 
        : CustomViewModelDialogBase<TViewModel>
        where TViewModel : CustomDialogViewModelBase
    {
        private TArgument Argument { get; }

        public CustomArgumentViewModelDialogBase()
        {
            if (Parameter is TArgument param)
                Argument = param;
        }
    }
}