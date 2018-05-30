using System;
using System.Windows.Input;
using Android.Views;
using AoLibs.Utilities.Android.Listeners;

namespace AoLibs.Utilities.Android
{
    public static class Extensions
    {
        public static void SetOnClickCommand(this View view, ICommand command)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(null)));
            command.CanExecuteChanged += (sender, args) => { view.Enabled = command.CanExecute(null); };
        }

        public static void SetOnClickCommand(this View view, ICommand command, Action<View,bool> onCanExecuteChanged)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(null)));
            onCanExecuteChanged(view,command.CanExecute(null));
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(null));        
        }

        public static void SetOnClickCommand(this View view, ICommand command, object arg)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(arg)));          
            command.CanExecuteChanged += (sender, args) => { view.Enabled = command.CanExecute(null); };           
        }

        public static void SetOnClickCommand(this View view, ICommand command, object arg,
            Action<View, bool> onCanExecuteChanged)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(arg)));
            onCanExecuteChanged(view, command.CanExecute(null));
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(null));
        }
    }
}