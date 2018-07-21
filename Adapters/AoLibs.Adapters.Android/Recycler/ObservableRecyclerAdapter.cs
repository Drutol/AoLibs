using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using Android.Support.V7.Widget;
using Android.Views;
using Object = Java.Lang.Object;

namespace AoLibs.Adapters.Android.Recycler
{
    /// <summary>
    /// RecyclerView adapter that allows to the define the collection using delegates.
    /// Additionally it subscribes to events that <see cref="ObservableCollection{T}"/> is emiting in order to update the recycler.
    /// </summary>
    /// <typeparam name="TItem">Type of item that list is compromised of.</typeparam>
    /// <typeparam name="THolder">ViewHolder for presented items.</typeparam>
    public class ObservableRecyclerAdapter<TItem, THolder> : RecyclerView.Adapter
        where THolder : RecyclerView.ViewHolder
    {
        private ConstructorInfo _holderConstructor;

        private IList<TItem> _dataSource;
        private INotifyCollectionChanged _notifier;
        /// <summary>
        /// Cache for automatic <see cref="BindingViewHolderBase{T}.ViewModel"/> property assignment.
        /// </summary>
        private readonly Dictionary<Type, PropertyInfo> _holderPropertyInfos = new Dictionary<Type, PropertyInfo>();

        /// <summary>
        /// Delegate which specifies how to fill-in the view with data contained within appropriate <see cref="TItem"/> instance.
        /// </summary>
        /// <typeparam name="THolder"></typeparam>
        /// <param name="item">Current item to get data from.</param>
        /// <param name="holder">Holder containing references to <see cref="View"/> instances.</param>
        /// <param name="position">Current adapter position.</param>
        public delegate void DataTemplateDelegate<THolder>(TItem item, THolder holder, int position);

        /// <summary>
        /// Delegate that creates holder based on given arguments.
        /// </summary>
        /// <typeparam name="T">Holder type.</typeparam>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <param name="view">Current view we are creating holder for.</param>
        /// <returns></returns>
        public delegate T HolderFactoryDelegate<out T>(ViewGroup parent, int viewType, View view);

        /// <summary>
        /// Delegate which provides inflated <see cref="View"/>.
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public delegate View ItemTemplateDelegate(int viewType);

        /// <summary>
        /// Special delegate that specifies more concrete types for <see cref="TItem"/> and <see cref="THolder"/> generic types.
        /// Used by <see cref="ObservableRecyclerAdapterWithMultipleViewTypes{TItemBase,THolder}.SpecializedItemEntry{TSpecializedItem,TSpecializedHolder}"/>
        /// </summary>
        /// <typeparam name="TSpecializedHolder"></typeparam>
        /// <typeparam name="TSpecializedItem"></typeparam>
        /// <param name="item"></param>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        public delegate void SpecializedDataTemplateDelegate<in TSpecializedHolder, in TSpecializedItem>(
            TSpecializedItem item,
            TSpecializedHolder holder,
            int position)
            where TSpecializedHolder : THolder where TSpecializedItem : TItem;

        /// <summary>
        /// Defines how to bind collection item to view.
        /// </summary>
        public DataTemplateDelegate<THolder> DataTemplate { get; set; }
        /// <summary>
        /// Defines how to create ViewHolder, can be left null if your ViewHolder has public constructor with one <see cref="View"/> argument.
        /// </summary>
        public HolderFactoryDelegate<THolder> HolderFactory { get; set; }
        /// <summary>
        /// Defines how to inflate layout for cell.
        /// </summary>
        public ItemTemplateDelegate ItemTemplate { get; set; }

        /// <summary>
        /// After inflating <see cref="View"/> assigns <see cref="ViewGroup.LayoutParams.MatchParent"/> width layout paramter.
        /// </summary>
        public bool StretchContentHorizonatally { get; set; }
        /// <summary>
        /// After inflating <see cref="View"/> assigns <see cref="ViewGroup.LayoutParams.MatchParent"/> height layout paramter.
        /// </summary>
        public bool StretchContentVertically { get; set; }
        /// <summary>
        /// Checks if given ViewHolder type is assignable from <see cref="BindingViewHolderBase{T}"/> if it is, automatically assigns current collection item to <see cref="BindingViewHolderBase{T}.ViewModel"/> in order to trigger binding. 
        /// </summary>
        public bool IsAutomaticViewModelBindingEnabled { get; set; }

        /// <summary>
        /// Creates new adapter instance.
        /// </summary>
        /// <param name="items">Backing data for the adapter.</param>
        /// <param name="dataTemplate">View binding definition.</param>
        /// <param name="itemTemplate">View inflation definition.</param>
        /// <param name="holderFactory">If not assigned make sure your ViewHolder has public constructo with <see cref="View"/> argument.</param>
        public ObservableRecyclerAdapter(IList<TItem> items, DataTemplateDelegate<THolder> dataTemplate,
            ItemTemplateDelegate itemTemplate,
            HolderFactoryDelegate<THolder> holderFactory = null)
        {
            DataTemplate = dataTemplate;
            HolderFactory = holderFactory ?? DefaultHolderFactory;
            ItemTemplate = itemTemplate;

            DataSource = items;
        }

        internal ObservableRecyclerAdapter()
        {

        }

        /// <summary>
        /// Current collection that adapter grabs its data from.
        /// </summary>
        public IList<TItem> DataSource
        {
            get => _dataSource;
            set
            {
                if (Equals(_dataSource, value))
                {
                    return;
                }

                if (_notifier != null)
                {
                    _notifier.CollectionChanged -= HandleCollectionChanged;
                }

                _dataSource = value;
                _notifier = value as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _notifier.CollectionChanged += HandleCollectionChanged;
                }

                NotifyDataSetChanged(); // Reload everything
            }
        }

        public override int ItemCount => _dataSource?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (IsAutomaticViewModelBindingEnabled)
            {
                var type = holder.GetType();
                if (!_holderPropertyInfos.ContainsKey(type))
                {
                    if (type.BaseType != null && type.BaseType.IsGenericType &&
                        type.BaseType.GetGenericTypeDefinition() == typeof(BindingViewHolderBase<>))
                    {
                        _holderPropertyInfos.Add(type, type.GetProperty(nameof(BindingViewHolderBase<object>.ViewModel)));
                    }
                    else
                    {
                        _holderPropertyInfos.Add(type, null);
                    }
                }

                if (_holderPropertyInfos[type] != null)
                    _holderPropertyInfos[type].SetValue(holder, _dataSource[position]);
            }

            DataTemplate(_dataSource[position], (THolder) holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = ItemTemplate(viewType);
            view.LayoutParameters = GetLayoutParameters(view);
            return HolderFactory(parent, viewType, view);
        }

        /// <summary>
        /// Detaches bindings from <see cref="BindingViewHolderBase{T}"/>
        /// </summary>
        /// <param name="holder"></param>
        public override void OnViewRecycled(Object holder)
        {
            if (holder is BindingViewHolderNonGenericBase bindingHolder)            
                bindingHolder.DetachBindings();          
        }

        /// <summary>
        /// Tries to find appropriate constructor for ViewHolder.
        /// </summary>
        /// <param name="viewGroup"></param>
        /// <param name="viewType"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when <see cref="THolder"/> is missing constructor with single <see cref="View"/> argument.</exception>
        private THolder DefaultHolderFactory(ViewGroup viewGroup, int viewType, View view)
        {
            try
            {
                if (_holderConstructor == null)
                    _holderConstructor = typeof(THolder).GetConstructor(new[] { typeof(View) });

                return (THolder)_holderConstructor.Invoke(new object[] { view });
            }
            catch (Exception)
            {
                throw new ArgumentException($"Given ViewHolder type ({typeof(THolder)}) is missing constructor with single Android.Views.View argument.");
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    var count = e.NewItems.Count;
                    for (var i = 0; i < count; i++)
                    {
                        NotifyItemInserted(e.NewStartingIndex + i);
                    }
                }
                    break;
                case NotifyCollectionChangedAction.Remove:
                {
                    var count = e.OldItems.Count;
                    for (var i = 0; i < count; i++)
                    {
                        NotifyItemRemoved(e.OldStartingIndex + i);
                    }
                }
                    break;
                default:
                    NotifyDataSetChanged();
                    break;
            }
        }

        protected RecyclerView.LayoutParams GetLayoutParameters(View view)
        {
            var width = StretchContentHorizonatally ? ViewGroup.LayoutParams.MatchParent : view.LayoutParameters.Width;
            var height = StretchContentVertically ? ViewGroup.LayoutParams.MatchParent : view.LayoutParameters.Height;

            return new RecyclerView.LayoutParams(width, height);
        }
    }
}