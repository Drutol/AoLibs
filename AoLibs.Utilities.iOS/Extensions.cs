using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using UIKit;

namespace AoLibs.Utilities.iOS
{
    public static class ClickCommandExtensions
    {
        private const string GestureRecognizerName = "AoLibsTapGestureRecognizer";

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and alters <see cref="View.Enabled"/> according to <see cref="ICommand.CanExecute"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        public static void SetOnClickCommand(this UIView view, ICommand command)
        {
            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(recognizer => recognizer.Name == GestureRecognizerName);
            if(tapGestureRecognizer != null)
                view.RemoveGestureRecognizer(tapGestureRecognizer);
            tapGestureRecognizer = new UITapGestureRecognizer(() => command.Execute(null))
            {
                NumberOfTapsRequired = 1,
                Name = GestureRecognizerName
            };
            view.UserInteractionEnabled = command.CanExecute(null);
            view.AddGestureRecognizer(tapGestureRecognizer);
            command.CanExecuteChanged += (sender, args) => { view.UserInteractionEnabled = command.CanExecute(null); };
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and allows to easily alter the View with <see cref="onCanExecuteChanged"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="onCanExecuteChanged">Delegate allowing custom modification of the view.</param>
        public static void SetOnClickCommand(this UIView view, ICommand command, Action<UIView, bool> onCanExecuteChanged)
        {
            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(recognizer => recognizer.Name == GestureRecognizerName);
            if (tapGestureRecognizer != null)
                view.RemoveGestureRecognizer(tapGestureRecognizer);
            tapGestureRecognizer = new UITapGestureRecognizer(() => command.Execute(null))
            {
                NumberOfTapsRequired = 1,
                Name = GestureRecognizerName
            };
            view.UserInteractionEnabled = command.CanExecute(null);
            view.AddGestureRecognizer(tapGestureRecognizer);
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(null));
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/> with arrgument <see cref="arg"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and alters <see cref="View.Enabled"/> according to <see cref="ICommand.CanExecute"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="arg">Argument to pass to the <see cref="ICommand.Execute"/></param>
        public static void SetOnClickCommand(this UIView view, ICommand command, object arg)
        {
            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(recognizer => recognizer.Name == GestureRecognizerName);
            if (tapGestureRecognizer != null)
                view.RemoveGestureRecognizer(tapGestureRecognizer);
            tapGestureRecognizer = new UITapGestureRecognizer(() => command.Execute(null))
            {
                NumberOfTapsRequired = 1,
                Name = GestureRecognizerName
            };
            view.UserInteractionEnabled = command.CanExecute(null);
            view.AddGestureRecognizer(tapGestureRecognizer);
            command.CanExecuteChanged += (sender, args) => { view.UserInteractionEnabled = command.CanExecute(null); };
        }

        /// <summary>
        /// Sets <see cref="OnClickListener"/> executing given <see cref="command"/> with arrgument <see cref="arg"/>.
        /// Addtionally hooks to <see cref="ICommand.CanExecuteChanged"/> and allows to easily alter the View with <see cref="onCanExecuteChanged"/>
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="arg">Argument to pass to the <see cref="ICommand.Execute"/></param>
        /// <param name="onCanExecuteChanged">Delegate allowing custom modification of the view.</param>
        public static void SetOnClickCommand(
            this UIView view,
            ICommand command,
            object arg,
            Action<UIView, bool> onCanExecuteChanged)
        {
            var tapGestureRecognizer = view.GestureRecognizers.FirstOrDefault(recognizer => recognizer.Name == GestureRecognizerName);
            if (tapGestureRecognizer != null)
                view.RemoveGestureRecognizer(tapGestureRecognizer);
            tapGestureRecognizer = new UITapGestureRecognizer(() => command.Execute(arg))
            {
                NumberOfTapsRequired = 1,
                Name = GestureRecognizerName
            };
            view.UserInteractionEnabled = command.CanExecute(arg);
            view.AddGestureRecognizer(tapGestureRecognizer);
            command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(arg));
        }
    }
}
