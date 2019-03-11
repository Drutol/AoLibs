using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AoLibs.Adapters.Core.Interfaces;
using AoLibs.Dialogs.Core.Interfaces;
using AoLibs.Navigation.Core.Interfaces;
using AoLibs.Sample.Shared.BL;
using AoLibs.Sample.Shared.Interfaces;
using AoLibs.Sample.Shared.Models;
using AoLibs.Sample.Shared.NavArgs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace AoLibs.Sample.Shared.ViewModels
{
    public class TestViewModelA : ViewModelBase
    {
        private readonly List<ISomeFancyProvider> _fancyProviders;
        private readonly IMessageBoxProvider _messageBoxProvider;
        private readonly IPickerAdapter _pickerAdapter;
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly ICustomDialogsManager<DialogIndex> _dialogsManager;
        private readonly AppVariables _appVariables;
        private UserResponse _userResponse;

        private bool _dialogBShouldShowLongText = true;
        private string _dialogBLongText = "This is very long text in a multiline label, so you can see the TestDialogB's height can be adjusted based on it's content, dynamically at inital binding time.\r\nAlso the text in the label above is long, but the label is single line, so you can see how it affects the dialog's width, but never extends it beyond the margins. Note that the 15pt margins that the dialog has, are defined on client side. Have a nice day!";
        private string _dialogBShortText = "And this is short text, to show you how tiny this dialog can be.";

        public TestViewModelA(
            IEnumerable<ISomeFancyProvider> fancyProviders,
            IMessageBoxProvider messageBoxProvider,
            IPickerAdapter pickerAdapter,
            INavigationManager<PageIndex> navigationManager,
            ICustomDialogsManager<DialogIndex> dialogsManager,
            AppVariables appVariables)
        {
            _fancyProviders = fancyProviders.ToList();
            _messageBoxProvider = messageBoxProvider;
            _pickerAdapter = pickerAdapter;
            _navigationManager = navigationManager;
            _dialogsManager = dialogsManager;
            _appVariables = appVariables;

            ShowLastFanciedThingCommand = new RelayCommand(
                async () =>
                {
                    await _messageBoxProvider.ShowMessageBoxOkAsync(
                        "The thing you fancy!",
                        $"You fancied: {_appVariables.UserResponse.Value.FancyThing}\nAt {_appVariables.UserResponse.Value.DateTime}",
                        "Yeah, that's fancy.");
                }, () => UserResponse != null);
        }

        public void NavigatedTo()
        {
            UserResponse = _appVariables.UserResponse.Value;
        }

        public UserResponse UserResponse
        {
            get => _userResponse;
            set
            {
                _userResponse = value;
                _appVariables.UserResponse.Value = value;
                RaisePropertyChanged();
                ShowLastFanciedThingCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ShowLastFanciedThingCommand { get; }

        public RelayCommand AskUserAboutFancyThingsCommand => new RelayCommand(async () =>
        {
            var choices = _fancyProviders.Select(provider => provider.SomethingFancy).ToList();

            var choice = await _pickerAdapter.ShowItemsPicker(
                choices,
                UserResponse == null ? -1 : choices.IndexOf(UserResponse.FancyThing),
                "Choose something fancy",
                "Nope",
                "That's fancy!");

            if (choice != null)
            {
                UserResponse = new UserResponse
                {
                    FancyThing = _fancyProviders[choice.Value].SomethingFancy,
                    DateTime = DateTime.UtcNow,
                };
            }
        });

        public RelayCommand ResetFanciness =>
            new RelayCommand(() => { UserResponse = null; });

        public RelayCommand NavigateSomewhereElseCommand =>
            new RelayCommand(() => _navigationManager.Navigate(
                PageIndex.PageB,
                new PageBNavArgs {Message = "Hello from A!"}));

        public RelayCommand ShowDialogCommand =>
            new RelayCommand(async () =>
            {
                var result = await _dialogsManager[DialogIndex.TestDialogA].AwaitResult<int>();
            });

        public RelayCommand ShowDialogBCommand =>
            new RelayCommand(() =>
            {
                _dialogsManager[DialogIndex.TestDialogB].Show(new DialogBNavArgs {Message = _dialogBShouldShowLongText ? _dialogBLongText : _dialogBShortText });
                _dialogBShouldShowLongText = !_dialogBShouldShowLongText;
            });

        public RelayCommand NavigateCameraCommand =>
            new RelayCommand(() => _navigationManager.Navigate(PageIndex.PageCamera));
    }
}
