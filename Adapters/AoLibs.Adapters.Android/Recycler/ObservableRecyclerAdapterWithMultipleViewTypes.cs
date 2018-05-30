using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;

namespace AoLibs.Adapters.Android.Recycler
{
    [Preserve(AllMembers = true)]
    public class
        ObservableRecyclerAdapterWithMultipleViewTypes<TItemBase, THolder> : ObservableRecyclerAdapter<TItemBase,
            THolder> where THolder : RecyclerView.ViewHolder
    {
        private readonly Dictionary<Type, IItemEntry> _templates;

        public interface IItemEntry
        {
            DataTemplateDelegate<THolder> DataTemplate { get; set; }
            HolderFactoryDelegate<THolder> HolderFactory { get; set; } 
            ItemTemplateDelegate ItemTemplate { get; set; }
        }

        public class ItemEntry : IItemEntry
        {
            public ItemEntry()
            {
                HolderFactory = DefaultHolderFactory;
            }

            private ConstructorInfo _constructor;

            public DataTemplateDelegate<THolder> DataTemplate { get; set; }
            public HolderFactoryDelegate<THolder> HolderFactory { get; set; }

            private THolder DefaultHolderFactory(ViewGroup viewGroup, int viewType, View view)
            {
                if (_constructor == null)
                    _constructor = typeof(THolder).GetConstructor(new[] {typeof(View)});
                

                return (THolder)_constructor.Invoke(new object[] {view});
            }

            public ItemTemplateDelegate ItemTemplate { get; set; }
        }

        public class SpecializedItemEntry<TSpecializedItem, TSpecializedHolder> : IItemEntry
            where TSpecializedItem : TItemBase
            where TSpecializedHolder : THolder
        {
            private HolderFactoryDelegate<THolder> _holderFactory;
            private DataTemplateDelegate<THolder> _dataTemplate;
            private ConstructorInfo _constructor;

            public SpecializedItemEntry()
            {
                SpecializedHolderFactory = DefaultHolderFactory;
            }


            public SpecializedDataTemplateDelegate<TSpecializedHolder, TSpecializedItem> SpecializedDataTemplate
            {
                get;
                set;
            }

            public SpecializedHolderFactoryDelegate<TSpecializedHolder> SpecializedHolderFactory { get; set; }  

            public DataTemplateDelegate<THolder> DataTemplate
            {
                get => _dataTemplate ?? (_dataTemplate =
                           (item, holder, position) =>
                           {
                               SpecializedDataTemplate((TSpecializedItem) item, (TSpecializedHolder) holder, position);
                           });
                set => throw new InvalidOperationException("Use Specialized factory or base class instead.");
            }

            public HolderFactoryDelegate<THolder> HolderFactory
            {
                get => _holderFactory ?? (_holderFactory =
                           (parent, viewtype, view) => SpecializedHolderFactory(parent,
                               viewtype, view));
                set => throw new InvalidOperationException("Use Specialized factory or base class instead.");
            }

            private TSpecializedHolder DefaultHolderFactory(ViewGroup viewGroup, int viewType, View view)
            {
                if (_constructor == null)
                    _constructor = typeof(TSpecializedHolder).GetConstructor(new[] { typeof(View) });

                return (TSpecializedHolder)_constructor.Invoke(new object[] { view });
            }


            public ItemTemplateDelegate ItemTemplate { get; set; }
        }

        public ObservableRecyclerAdapterWithMultipleViewTypes(Dictionary<Type, IItemEntry> templates,
            IList<TItemBase> items)
        {
            _templates = templates;

            DataSource = items;
        }

        public override int GetItemViewType(int position)
        {
            return _dataSource[position].GetType().GetHashCode();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DataTemplate = _templates[_dataSource[position].GetType()].DataTemplate;
            base.OnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var entry = _templates.First(pair => pair.Key.GetHashCode() == viewType);
            HolderFactory = entry.Value.HolderFactory;
            ItemTemplate = entry.Value.ItemTemplate;
            return base.OnCreateViewHolder(parent, viewType);
        }
    }
}