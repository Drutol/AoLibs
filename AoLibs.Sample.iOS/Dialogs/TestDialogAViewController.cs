using System;
using AoLibs.Dialogs.iOS;
using AoLibs.Sample.iOS.Utils;
using AoLibs.Sample.Shared.Models;

namespace AoLibs.Sample.iOS
{
    [CustomDialog((int)DialogIndex.TestDialogA, Constants.MainDialogsStoryboard, nameof(TestDialogAViewController))]
    public partial class TestDialogAViewController : CustomDialogBase
    {
        public TestDialogAViewController (IntPtr handle) : base (handle)
        {
        }
    }
}