using System.Reflection;
using AoLibs.Dialogs.Core;
using AoLibs.Dialogs.Core.Interfaces;
using UIKit;

namespace AoLibs.Dialogs.iOS
{
    /// <inheritdoc />
    public class StoryboardOneshotDialogProvider<TPage> : OneshotCustomDialogProvider<TPage> 
        where TPage : class, ICustomDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotDialogProvider{TPage}"/> class.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        /// <param name="storyboardName">Name of the storyboard file.</param>
        /// <param name="viewControllerIdentifier">Name of the controller within the storyboard.</param>
        public StoryboardOneshotDialogProvider(string storyboardName, string viewControllerIdentifier)
        {
            SetUpFactory(storyboardName, viewControllerIdentifier);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotDialogProvider{TPage}"/> class.
        /// Builds ViewController based on data contained in <see cref="CustomDialogAttribute"/> attached to <see cref="TPage"/>.
        /// <see cref="UIStoryboard.FromName"/> and <see cref="UIStoryboard.InstantiateInitialViewController"/> or <see cref="UIStoryboard.InstantiateViewController"/> is used to create the controller.
        /// </summary>
        public StoryboardOneshotDialogProvider() 
            : this(typeof(TPage).GetTypeInfo().GetCustomAttribute<CustomDialogAttribute>())
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryboardOneshotDialogProvider{TPage}"/> class.
        /// Extracts data from <see cref="attr"/> to prepare the provider.
        /// </summary>
        /// <param name="attr">Page attribute.</param>
        public StoryboardOneshotDialogProvider(CustomDialogAttribute attr)
        {
            SetUpFactory(attr.StoryboardName, attr.ViewControllerIdentifier);
        }

        private void SetUpFactory(string storyboardName, string viewControllerIdentifier)
        {
            if (string.IsNullOrEmpty(viewControllerIdentifier))
            {
                Factory = () => UIStoryboard.FromName(storyboardName, null)
                    .InstantiateInitialViewController() as TPage;
            }
            else
            {
                Factory = () =>
                    UIStoryboard.FromName(storyboardName, null)
                        .InstantiateViewController(viewControllerIdentifier) as TPage;
            }
        }
    }
}