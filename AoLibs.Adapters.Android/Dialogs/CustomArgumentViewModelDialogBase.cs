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
    /// <summary>
    /// Dialog base with possibility of providing the type of ViewModel associated with given dialog, and parameter it will be using.
    /// </summary>
    /// <typeparam name="TViewModel">The ViewModel.</typeparam>
    /// <typeparam name="TArgument">The Argument.</typeparam>
    public abstract class CustomArgumentViewModelDialogBase<TViewModel,TArgument> 
        : CustomViewModelDialogBase<TViewModel>
        where TViewModel : CustomDialogViewModelBase
    {
        private TArgument Argument { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomArgumentViewModelDialogBase{TViewModel, TArgument}"/> class.
        /// </summary>
        protected CustomArgumentViewModelDialogBase()
        {
            if (Parameter is TArgument param)
                Argument = param;
        }
    }
}