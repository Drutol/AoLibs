using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    /// <summary>
    /// Class that presents system picker allowing user to choose from given collection of items.
    /// </summary>
    public class PickerAdapter : IPickerAdapter
    {
        private readonly IContextProvider _contextProvider;

        public PickerAdapter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task<int?> ShowItemsPicker(IEnumerable<string> items, int selectedIndex, string title, string cancelText, string okText)
        {
            var semaphore = new SemaphoreSlim(0);
            var builder = new AlertDialog.Builder(_contextProvider.CurrentContext);
            int? selectedItem = selectedIndex;
            builder.SetTitle(title);
            builder.SetSingleChoiceItems(items.ToArray(), selectedIndex, (sender, args) =>
            {
                selectedItem = args.Which;
            });
            builder.SetNegativeButton(cancelText, (sender, args) =>
            {
                selectedItem = null;
                semaphore.Release();
            });
            builder.SetPositiveButton(okText, (sender, args) => semaphore.Release());

            var dialog = builder.Create();
            dialog.SetCanceledOnTouchOutside(false);
            dialog.SetCancelable(false);
            dialog.Show();

            await semaphore.WaitAsync();
            dialog.Dismiss();

            return selectedItem;
        }

        public async Task<DateTime?> ShowDatePicker(DateTime startingDate, string okText, string cancelText = null)
        {
            var semaphore = new SemaphoreSlim(0);
            DateTime? selectedDate = null;
            var dpd = new ListenableDatePickerDialog(
                _contextProvider.CurrentContext,
                new DateSetListener(tuple => { }),
                startingDate.Year, 
                startingDate.Month - 1,
                startingDate.Day)
            {
                Callback = tuple =>
                {
                    selectedDate =
                        new DateTime(tuple.year, tuple.monthOfYear, tuple.dayOfMonth, 0, 0, 0);
                    semaphore.Release();
                }
            };

            dpd.SetButton((int)DialogButtonType.Positive,okText, (sender, args) => semaphore.Release());
            dpd.SetButton((int)DialogButtonType.Negative,cancelText, (sender, args) => semaphore.Release());
            dpd.CancelEvent += (sender, args) => semaphore.Release();            
            dpd.Show();

            await semaphore.WaitAsync();
            dpd.Dismiss();
            return selectedDate;
        }

       private class ListenableDatePickerDialog : DatePickerDialog
        {
            public Action<(int year, int monthOfYear, int dayOfMonth)> Callback { get; set; }

            public ListenableDatePickerDialog(Context context, EventHandler<DateSetEventArgs> callBack, int year, int monthOfYear, int dayOfMonth) 
                : base(context, callBack, year, monthOfYear, dayOfMonth)
            {
            }

            public ListenableDatePickerDialog(Context context, int theme, EventHandler<DateSetEventArgs> callBack, int year, int monthOfYear, int dayOfMonth) 
                : base(context, theme, callBack, year, monthOfYear, dayOfMonth)
            {
            }

            protected ListenableDatePickerDialog(IntPtr javaReference, JniHandleOwnership transfer) 
                : base(javaReference, transfer)
            {
            }

            public ListenableDatePickerDialog(Context context) 
                : base(context)
            {
            }

            public ListenableDatePickerDialog(Context context, IOnDateSetListener listener, int year, int month, int dayOfMonth) 
                : base(context, listener, year, month, dayOfMonth)
            {
            }

            public ListenableDatePickerDialog(Context context, int themeResId) 
                : base(context, themeResId)
            {
            }

            public ListenableDatePickerDialog(Context context, int themeResId, IOnDateSetListener listener, int year, int monthOfYear, int dayOfMonth) 
                : base(context, themeResId, listener, year, monthOfYear, dayOfMonth)
            {
            }

            public override void OnDateChanged(DatePicker view, int year, int month, int dayOfMonth)
            {
                Callback.Invoke((year, month + 1, dayOfMonth));
            }
        }

        private class DateSetListener : Java.Lang.Object, global::Android.App.DatePickerDialog.IOnDateSetListener
        {
            private Action<(int year, int monthOfYear, int dayOfMonth)> _callback;

            public DateSetListener(Action<(int year, int monthOfYear, int dayOfMonth)> callback)
            {
                _callback = callback;
            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                _callback.Invoke((year, monthOfYear + 1, dayOfMonth));
            }
        }
    }
}