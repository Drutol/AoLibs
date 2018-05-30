using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace NavigationLib.Android.Navigation
{
    public class NavigationManager<TPageIdentifier> : NavigationManagerBase<NavigationFragmentBase, TPageIdentifier>
    {
        private readonly FragmentManager _fragmentManager;
        private readonly ViewGroup _rootFrame;
        private readonly Action<FragmentTransaction> _interceptTransaction;

        public NavigationManager(FragmentManager fragmentManager, ViewGroup rootFrame,
            Dictionary<TPageIdentifier, IPageProvider<NavigationFragmentBase>> pageDefinitions,
            IStackResolver<NavigationFragmentBase, TPageIdentifier> stackResolver = null,
            Action<FragmentTransaction> interceptTransaction = null) : base(pageDefinitions,
            stackResolver)
        {
            _fragmentManager = fragmentManager;
            _rootFrame = rootFrame;
            _interceptTransaction = interceptTransaction;
        }

        public override void CommitPageTransaction(NavigationFragmentBase page)
        {
            try
            {
                var transaction = _fragmentManager.BeginTransaction();
                _interceptTransaction?.Invoke(transaction);
                transaction.Replace(_rootFrame.Id, page)
                           .DisallowAddToBackStack();
                transaction.CommitNowAllowingStateLoss();
                                           
                _fragmentManager.ExecutePendingTransactions();
            }
            catch (Exception e)
            {

            }
        }
    }
}