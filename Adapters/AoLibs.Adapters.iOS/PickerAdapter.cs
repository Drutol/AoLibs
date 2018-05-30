using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;
using UIKit;

//using DT.iOS.DatePickerDialog;

namespace AoLibs.Adapters.iOS
{
    public class PickerAdapter : IPickerAdapter
    {
        public async Task<DateTime?> ShowDatePicker(DateTime startingDate, string okText, string cancelText = null)
        {
            var semaphore = new SemaphoreSlim(0);
            DateTime? returnValue = null;

            //TODO: commented - reference not found.
            //var dialog = new DatePickerDialog();
                
            //dialog.Show(string.Empty, okText, cancelText, UIDatePickerMode.Date, time =>
            //{
            //    returnValue = time;
            //    semaphore.Release();
            //}, DateTime.Now);

            //await semaphore.WaitAsync();
            
            return returnValue;
        }

        public async Task<int?> ShowItemsPicker(IEnumerable<string> items, int selectedIndex, string title, string cancelText, string okText)
        {
            var semaphore = new SemaphoreSlim(0);
            int? returnValue = null;

            var alert = UIAlertController.Create(title, string.Empty, UIAlertControllerStyle.ActionSheet);

            foreach (var item in items)
            {
                alert.AddAction(UIAlertAction.Create(item, UIAlertActionStyle.Default, (x) => 
                {
                    returnValue = Array.IndexOf(alert.Actions, x);
                    semaphore.Release();
                }));
            }

            alert.AddAction(UIAlertAction.Create(cancelText, UIAlertActionStyle.Cancel, x => 
            {
                semaphore.Release();
            }));

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);

            await semaphore.WaitAsync();

            return returnValue;
        }
    }
}
