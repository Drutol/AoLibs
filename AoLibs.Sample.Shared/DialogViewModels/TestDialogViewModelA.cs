using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Adapters.Core.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.DialogViewModels
{
    public class TestDialogViewModelA : CustomDialogViewModelBase
    {
        private int _counter;

        public override void OnDialogAppeared()
        {

        }

        public override void OnDialogDismissed()
        {

        }

        public override CustomDialogConfig CustomDialogConfig { get; } = new CustomDialogConfig
        {
            Gravity = CustomDialogConfig.DialogGravity.Top | CustomDialogConfig.DialogGravity.Left,
            IsCancellable = false,
            StretchHorizontally = true,
            StretchVertically = false,
        };

        public int Counter
        {
            get => _counter;
            set
            {
                _counter = value;
                RaisePropertyChanged();
            }
        }

        public ICommand IncrementCommand => new RelayCommand(() =>
        {
            Counter++;
            Dialog.Hide();
            Dialog.SetResult(Counter);
        });
    }
}
