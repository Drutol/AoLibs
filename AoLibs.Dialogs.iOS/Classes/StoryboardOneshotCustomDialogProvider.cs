using System.Reflection;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    /// <summary>
    /// Provides <see cref="OneshotCustomDialogProvider{TDialog}"/> functionality for dialogs defined within storyboards by changing default factory delegates.
    /// The dialog needs to be marked with <see cref="CustomDialogAttribute"/>.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type.</typeparam>
    public class StoryboardOneshotCustomDialogProvider<TDialog> : OneshotCustomDialogProvider<TDialog> 
        where TDialog : class, ICustomDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotCustomDialogProvider{TDialog}"/> class.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        /// <param name="storyboardName">Name of the storyboard file.</param>
        /// <param name="viewControllerIdentifier">Name of the controller within the storyboard.</param>
        public StoryboardOneshotCustomDialogProvider(string storyboardName, string viewControllerIdentifier)
        {
            SetUpFactory(storyboardName, viewControllerIdentifier);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotCustomDialogProvider{TDialog}"/> class.
        /// Builds ViewController based on data contained in <see cref="CustomDialogAttribute"/> attached to dialog.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        public StoryboardOneshotCustomDialogProvider() 
            : this(typeof(TDialog).GetTypeInfo().GetCustomAttribute<CustomDialogAttribute>())
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotCustomDialogProvider{TDialog}"/> class.
        /// Extracts data from <see cref="CustomDialogAttribute"/> to prepare the provider.
        /// </summary>
        /// <param name="attr">Dialog attribute.</param>
        public StoryboardOneshotCustomDialogProvider(CustomDialogAttribute attr)
        {
            SetUpFactory(attr.StoryboardName, attr.ViewControllerIdentifier);
        }

        private void SetUpFactory(string storyboardName, string viewControllerIdentifier)
        {
            if (string.IsNullOrEmpty(viewControllerIdentifier))
            {
                Factory = () => UIStoryboard.FromName(storyboardName, null)
                    .InstantiateInitialViewController() as TDialog;
            }
            else
            {
                Factory = () =>
                    UIStoryboard.FromName(storyboardName, null)
                        .InstantiateViewController(viewControllerIdentifier) as TDialog;
            }
        }
    }
}