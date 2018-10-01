using System;
using System.Linq;
using System.Windows.Input;
using UIKit;

namespace AoLibs.Utilities.iOS.Extensions
{
    public static class ClickCommandExtensions
    {
        private const string GestureRecognizerName = "AoLibsTapGestureRecognizer";

        /// <summary>
        ///     Sets <see cref="UIGestureRecognizer" /> or <see cref="UIControl.TouchUpInside"/> executing given <see cref="command" />.
        ///     Additionally hooks to <see cref="ICommand.CanExecuteChanged" /> and alters
        ///     <see cref="UIView.UserInteractionEnabled" /> according to <see cref="ICommand.CanExecute" />
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        public static void SetOnClickCommand(this UIView view, ICommand command)
        {
            if (view is UIControl control)
                Bind(control, command);
            else
                Bind(view, command);
        }

        /// <summary>
        ///     Sets <see cref="UIGestureRecognizer" /> or <see cref="UIControl.TouchUpInside"/> executing given <see cref="command" />.
        ///     Additionally hooks to <see cref="ICommand.CanExecuteChanged" /> and allows to easily alter the View with
        ///     <see cref="onCanExecuteChanged" />
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="onCanExecuteChanged">Delegate allowing custom modification of the view.</param>
        public static void SetOnClickCommand(
            this UIView view,
            ICommand command,
            Action<UIView, bool> onCanExecuteChanged)
        {
            if (view is UIControl control)
                Bind(control, command, null, onCanExecuteChanged);
            else
                Bind(view, command, null, onCanExecuteChanged);
        }

        /// <summary>
        ///     Sets <see cref="UIGestureRecognizer" /> or <see cref="UIControl.TouchUpInside"/> executing given <see cref="command" /> with argument <see cref="arg" />.
        ///     Additionally hooks to <see cref="ICommand.CanExecuteChanged" /> and alters
        ///     <see cref="UIView.UserInteractionEnabled" /> according to <see cref="ICommand.CanExecute" />
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="arg">Argument to pass to the <see cref="ICommand.Execute" /></param>
        public static void SetOnClickCommand(this UIView view, ICommand command, object arg)
        {
            if (view is UIControl control)
                Bind(control, command, arg);
            else
                Bind(view, command, arg);
        }

        /// <summary>
        ///     Sets <see cref="UIGestureRecognizer" /> or <see cref="UIControl.TouchUpInside"/> executing given <see cref="command" /> with argument <see cref="arg" />.
        ///     Additionally hooks to <see cref="ICommand.CanExecuteChanged" /> and allows to easily alter the View with
        ///     <see cref="onCanExecuteChanged" />
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="command">Command to bind to the button.</param>
        /// <param name="arg">Argument to pass to the <see cref="ICommand.Execute" /></param>
        /// <param name="onCanExecuteChanged">Delegate allowing custom modification of the view.</param>
        public static void SetOnClickCommand(
            this UIView view,
            ICommand command,
            object arg,
            Action<UIView, bool> onCanExecuteChanged)
        {
            if (view is UIControl control)
                Bind(control, command, arg, onCanExecuteChanged);
            else
                Bind(view, command, arg, onCanExecuteChanged);
        }

        private static void Bind(
            UIControl control,
            ICommand command,
            object arg = null,
            Action<UIView, bool> onCanExecuteChanged = null)
        {
            // not ideal but the most clean approach in this case
            control.TouchUpInside -= Handler;
            control.TouchUpInside += Handler;

            control.Enabled = command.CanExecute(arg);
            if (onCanExecuteChanged is null)
                command.CanExecuteChanged += (sender, args) => control.Enabled = command.CanExecute(arg);
            else
                command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(control, command.CanExecute(arg));

            void Handler(object sender, EventArgs e)
            {
                command.Execute(null);
            }
        }

        private static void Bind(
            UIView view,
            ICommand command,
            object arg = null,
            Action<UIView, bool> onCanExecuteChanged = null)
        {
            var tapGestureRecognizer =
                view.GestureRecognizers?.FirstOrDefault(recognizer => recognizer.Name == GestureRecognizerName);
            if (tapGestureRecognizer != null)
                view.RemoveGestureRecognizer(tapGestureRecognizer);
            tapGestureRecognizer = new UITapGestureRecognizer(() => command.Execute(arg))
            {
                NumberOfTapsRequired = 1,
                Name = GestureRecognizerName
            };
            view.UserInteractionEnabled = command.CanExecute(arg);
            view.AddGestureRecognizer(tapGestureRecognizer);
            if (onCanExecuteChanged is null)
            {
                command.CanExecuteChanged += (sender, args) =>
                {
                    view.UserInteractionEnabled = command.CanExecute(arg);
                };
            }
            else
            {
                command.CanExecuteChanged += (sender, args) => onCanExecuteChanged(view, command.CanExecute(arg));
            }
        }
    }
}