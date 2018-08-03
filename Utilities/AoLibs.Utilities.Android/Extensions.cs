using System;
using System.Windows.Input;
using Android.Views;
using AoLibs.Utilities.Android.Listeners;

namespace AoLibs.Utilities.Android
{
    public static class Extensions
    {
        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and alters <see cref="View.Enabled"/> according to <see cref="ICommand.CanExecute"/>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="command"></param>
        public static void SetOnClickCommand(this View view, ICommand command)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(null)));
            view.Enabled = command.CanExecute(null);
            command.CanExecuteChanged += (sender, args) => { view.Enabled = command.CanExecute(null); };
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and allows to easily alter the View with <see cref="onCanExecuteChanged"/>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="command"></param>
        /// <param name="onCanExecuteChanged"></param>
        public static void SetOnClickCommand(this View view, ICommand command, Action<View,bool> onCanExecuteChanged)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(null)));
            onCanExecuteChanged(view,command.CanExecute(null));
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(null));        
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/> with arrgument <see cref="arg"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and alters <see cref="View.Enabled"/> according to <see cref="ICommand.CanExecute"/>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="command"></param>
        /// <param name="arg"></param>
        public static void SetOnClickCommand(this View view, ICommand command, object arg)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(arg)));
            view.Enabled = command.CanExecute(arg);
            command.CanExecuteChanged += (sender, args) => { view.Enabled = command.CanExecute(arg); };           
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/> with arrgument <see cref="arg"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and allows to easily alter the View with <see cref="onCanExecuteChanged"/>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="command"></param>
        /// <param name="arg"></param>
        /// <param name="onCanExecuteChanged"></param>
        public static void SetOnClickCommand(this View view, ICommand command, object arg,
            Action<View, bool> onCanExecuteChanged)
        {
            view.SetOnClickListener(new OnClickListener(v => command.Execute(arg)));
            onCanExecuteChanged(view, command.CanExecute(arg));
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(arg));
        }
    }
}