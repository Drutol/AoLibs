using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using Debug = System.Diagnostics.Debug;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace NavigationLib.Android.Navigation
{
    /// <summary>
    /// Class that fulfills the purpose of executing actual navigation transactions.
    /// </summary>
    /// <typeparam name="TPageIdentifier"></typeparam>
    public class NavigationManager<TPageIdentifier> : NavigationManagerBase<NavigationFragmentBase, TPageIdentifier>
    {
        private readonly FragmentManager _fragmentManager;
        private readonly ViewGroup _rootFrame;
        private readonly Action<FragmentTransaction> _interceptTransaction;

        /// <summary>
        /// If the navigation fails, should an exception be thrown.
        /// </summary>
        public bool ThrowOnNavigationException { get; set; } = true;

        /// <summary>
        /// Creates new naviagtion manager.
        /// </summary>
        /// <param name="fragmentManager">Fragment manager of main activity.</param>
        /// <param name="rootFrame">The view which will be used as the one being replaced with new Views</param>
        /// <param name="pageDefinitions">The dictionary defining pages.</param>
        /// <param name="stackResolver">Class allowing to differentiate to which stack given indentigier belongs.</param>
        /// <param name="interceptTransaction">Delegate allowing to modify <see cref="FragmentTransaction"/> before commiting.</param>
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
                Console.WriteLine($"There was an issue navigating to idndicated page, please ensure everyhting is set-up correctly. Exception: {e}");
                if (ThrowOnNavigationException)
                    throw e;
            }
        }
    }
}