using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Android.Support.V7.Widget;
using Android.Views;
using Object = Java.Lang.Object;

namespace AoLibs.Adapters.Android.Recycler
{
    ////[ClassInfo(typeof(ObservableAdapter<T>)]
    public class ObservableRecyclerAdapter<TItem, THolder> : RecyclerView.Adapter
        where THolder : RecyclerView.ViewHolder
    {
        public delegate void DataTemplateDelegate<THolder>(TItem item, THolder holder, int position);

        public delegate T HolderFactoryDelegate<out T>(ViewGroup parent, int viewType, View view);

        public delegate View ItemTemplateDelegate(int viewType);

        public delegate void SpecializedDataTemplateDelegate<TSpecializedHolder, TSpecializedItem>(
            TSpecializedItem item, TSpecializedHolder holder,
            int position) where TSpecializedHolder : THolder where TSpecializedItem : TItem;

        public delegate T SpecializedHolderFactoryDelegate<out T>(ViewGroup parent, int viewType, View view);

        protected IList<TItem> _dataSource;
        private INotifyCollectionChanged _notifier;
        private int _oldPosition = -1;
        private View _oldView;
        private TItem _selectedItem;

        public DataTemplateDelegate<THolder> DataTemplate { get; set; }
        public HolderFactoryDelegate<THolder> HolderFactory { get; set; }
        public ItemTemplateDelegate ItemTemplate { get; set; }

        private readonly Dictionary<Type, PropertyInfo> _holderPropertyInfos = new Dictionary<Type, PropertyInfo>();
        private ConstructorInfo _holderConstructor;

        public bool ApplyLayoutParams { get; set; }

        public ObservableRecyclerAdapter()
        {

        }

        public ObservableRecyclerAdapter(IList<TItem> items, DataTemplateDelegate<THolder> dataTemplate,
            ItemTemplateDelegate itemTemplate,
            HolderFactoryDelegate<THolder> holderFactory = null)
        {
            DataTemplate = dataTemplate;
            HolderFactory = holderFactory ?? DefaultHolderFactory;
            ItemTemplate = itemTemplate;

            DataSource = items;
        }

        public IList<TItem> DataSource
        {
            get { return _dataSource; }

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
                    _holderPropertyInfos.Add(type,null);
                }
            }

            if(_holderPropertyInfos[type] != null)
                _holderPropertyInfos[type].SetValue(holder, _dataSource[position]);

            DataTemplate(_dataSource[position], (THolder) holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = ItemTemplate(viewType);
            if (ApplyLayoutParams)
                view.LayoutParameters = new RecyclerView.LayoutParams(-1, -2);
            return HolderFactory(parent, viewType, view);
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            void Act()
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

            Act();
        }

        private THolder DefaultHolderFactory(ViewGroup viewGroup, int viewType, View view)
        {
            if (_holderConstructor == null)
                _holderConstructor = typeof(THolder).GetConstructor(new[] { typeof(View) });

            return (THolder)_holderConstructor.Invoke(new object[] { view });
        }

        public override void OnViewRecycled(Object holder)
        {
            if (holder is BindingViewHolderNonGenericBase bindingHolder)            
                bindingHolder.DetachBindings();          
        }
    }
}