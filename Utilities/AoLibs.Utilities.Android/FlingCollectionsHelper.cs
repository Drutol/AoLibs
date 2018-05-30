using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AoLibs.Utilities.Android.Views;
using GalaSoft.MvvmLight.Helpers;

namespace AoLibs.Utilities.Android
{
    public static class FlingCollectionsHelper
    {
        public class FlingAdapterRegistration : IDisposable
        {
            private readonly AbsListView _view;
            public bool FlingState { get; set; }

            public FlingAdapterRegistration(AbsListView view)
            {
                _view = view;
            }

            public void Dispose()
            {
                ClearFlingAdapter();
            }

            public void ClearFlingAdapter()
            {
                _view.SetOnScrollListener(null);
                _view.Adapter = null;
            }
        }

        public class FlingAdapterRegistration<TViewHolder> : IDisposable
        {
            private readonly AbsListView _view;
            public bool FlingState { get; set; }
            public Dictionary<View, TViewHolder> ViewHolders { get; set; } = new Dictionary<View, TViewHolder>();

            public FlingAdapterRegistration(AbsListView view)
            {
                _view = view;
            }

            public void Dispose()
            {
                ClearFlingAdapter();
            }

            public void ClearFlingAdapter()
            {
                ViewHolders.Clear();
                ViewHolders = null;
                _view.SetOnScrollListener(null);
                _view.Adapter = null;
            }
        }

        public static FlingAdapterRegistration InjectFlingAdapter<T>(this AbsListView container, IList<T> items,
            Func<int, View> containerTemplate,
            Action<View, int, T> dataTemplateBasic,
            Action<View, int, T> dataTemplateFling,
            Action<View, int, T> dataTemplateFull,
            View footer = null, bool skipBugFix = false) where T : class
        {
            var registration = new FlingAdapterRegistration(container);

            container.MakeFlingAware(b =>
            {
                if (registration.FlingState == b)
                    return;
                registration.FlingState = b;
                if (!b)
                {
                    for (int i = 0; i < container.ChildCount; i++)
                    {
                        var view = container.GetChildAt(i);
                        var item = view.Tag.Unwrap<T>();
                        if (view.Tag?.ToString() == "Footer")
                            continue;
                        dataTemplateFull(view, items.IndexOf(item), item);
                    }
                }
            });

            container.Adapter = items.GetAdapter((i, arg2, arg3) =>
            {
                var root = arg3 ?? containerTemplate(i);
                root.Tag = arg2.Wrap();
                dataTemplateBasic?.Invoke(root, i, arg2);
                if (registration.FlingState)
                    dataTemplateFling(root, i, arg2);
                else
                    dataTemplateFull(root, i, arg2);
                return root;
            });


            return registration;
        }

        public static FlingAdapterRegistration<TViewHolder> InjectFlingAdapter<T, TViewHolder>(
            this AbsListView container, IList<T> items, 
            Func<View, TViewHolder> holderFactory,
            Func<int, View> containerTemplate,
            Action<View, int, T, TViewHolder> dataTemplateBasic,             
            Action<View, int, T, TViewHolder> dataTemplateFling,
            Action<View, int, T, TViewHolder> dataTemplateFull,        
            View footer = null, bool skipBugFix = false, Action onScrolled = null) where T : class
        {
            var registration = new FlingAdapterRegistration<TViewHolder>(container);
            if (onScrolled == null)
            {
                container.MakeFlingAware(b =>
                {
                    if (registration.FlingState == b)
                        return;
                    registration.FlingState = b;
                    if (!b)
                    {
                        for (int i = 0; i < container.ChildCount; i++)
                        {
                            var view = container.GetChildAt(i);
                            var item = view.Tag.Unwrap<T>();
                            if (view.Tag?.ToString() == "Footer")
                                continue;
                            dataTemplateFull(view, items.IndexOf(item), item, registration.ViewHolders[view]);
                        }
                    }
                });
            }
            else
            {
                container.MakeFlingAware(b =>
                {
                    onScrolled.Invoke();
                    if (registration.FlingState == b)
                        return;
                    registration.FlingState = b;
                    if (!b)
                    {
                        for (int i = 0; i < container.ChildCount; i++)
                        {
                            var view = container.GetChildAt(i);
                            var item = view.Tag.Unwrap<T>();
                            if (view.Tag?.ToString() == "Footer")
                                continue;
                            dataTemplateFull(view, items.IndexOf(item), item, registration.ViewHolders[view]);
                        }
                    }
                });
            }

                container.Adapter = items.GetAdapter((i, arg2, arg3) =>
                {
                    TViewHolder holder;
                    View root = null;
                    if (arg3 == null)
                    {
                        root = containerTemplate(i);
                        registration.ViewHolders[root] = holder = holderFactory(root);
                    }
                    else
                    {
                        root = arg3;
                        holder = registration.ViewHolders[root];
                    }
                    root.Tag = arg2.Wrap();
                    dataTemplateBasic.Invoke(root, i, arg2, holder);
                    if (registration.FlingState)
                        dataTemplateFling(root, i, arg2, holder);
                    else
                        dataTemplateFull(root, i, arg2, holder);
                    return root;
                });
           
            return registration;
        }

 
    }
}