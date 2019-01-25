using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Dialogs.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.DialogViewModels
{
    public class TestDialogViewModelA : CustomDialogViewModelBase
    {
        private int _counter;

        protected override void OnDialogAppeared()
        {
        }

        protected override void OnDialogDismissed()
        {
        }

        public override CustomDialogConfig CustomDialogConfig { get; } = new CustomDialogConfig
        {
            Gravity = CustomDialogConfig.DialogGravity.Bottom | CustomDialogConfig.DialogGravity.Center,
            IsCancellable = true,
            StretchHorizontally = false,
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
