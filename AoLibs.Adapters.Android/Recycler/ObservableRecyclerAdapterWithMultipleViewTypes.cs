using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;

namespace AoLibs.Adapters.Android.Recycler
{
    /// <summary>
    /// Recycler adapter that work the same way as <see cref="ObservableRecyclerAdapter{TItem,THolder}"/> but offers funcionality of defining <see cref="View"/> per <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="TItemBase">Base of the items found in the collection. Can be <see cref="Object"/> but I encourage to create separate (empty evn) interfaces for item groups.</typeparam>
    /// <typeparam name="THolder"></typeparam>
    /// <inheritdoc/>
    public class
        ObservableRecyclerAdapterWithMultipleViewTypes<TItemBase, THolder> : ObservableRecyclerAdapter<TItemBase,
            THolder> where THolder : RecyclerView.ViewHolder
    {
        private readonly Dictionary<Type, IItemEntry> _templates;

        /// <summary>
        /// Interface used to define how to represent given <see cref="TItemBase"/>.
        /// </summary>
        public interface IItemEntry
        {        
            /// <summary>
            /// Defines how to bind collection item to view.
            /// </summary>
            DataTemplateDelegate<THolder> DataTemplate { get; set; }
            /// <summary>
            /// Defines how to create ViewHolder, can be left null if your ViewHolder has public constructor with one <see cref="View"/> argument.
            /// </summary>
            HolderFactoryDelegate<THolder> HolderFactory { get; set; }
            /// <summary>
            /// Defines how to inflate layout for cell.
            /// </summary>
            ItemTemplateDelegate ItemTemplate { get; set; }
        }

        /// <summary>
        /// Simple implementation of <see cref="IItemEntry"/>
        /// </summary>
        public class ItemEntry : IItemEntry
        {
            private ConstructorInfo _constructor;

            public ItemEntry()
            {
                HolderFactory = DefaultHolderFactory;
            }

            /// <inheritdoc />
            public DataTemplateDelegate<THolder> DataTemplate { get; set; }
            /// <inheritdoc />
            public HolderFactoryDelegate<THolder> HolderFactory { get; set; }
            /// <inheritdoc />
            public ItemTemplateDelegate ItemTemplate { get; set; }

            private THolder DefaultHolderFactory(ViewGroup viewGroup, int viewType, View view)
            {
                try
                {
                    if (_constructor == null)
                        _constructor = typeof(THolder).GetConstructor(new[] { typeof(View) });

                    return (THolder)_constructor.Invoke(new object[] { view });
                }
                catch (Exception)
                {
                    throw new ArgumentException($"Given ViewHolder type ({typeof(THolder)}) is missing constructor with single Android.Views.View argument.");
                }
            }
        }

        /// <summary>
        /// Implementation of <see cref="IItemEntry"/> that alows to specify concrete types for <see cref="TItemBase"/> and <see cref="THolder"/>.
        /// This way we are avoiding manual casting in the delegate methods such as <see cref="DataTemplateDelegate{THolder}"/>
        /// </summary>
        /// <typeparam name="TSpecializedItem"></typeparam>
        /// <typeparam name="TSpecializedHolder"></typeparam>
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

            public HolderFactoryDelegate<TSpecializedHolder> SpecializedHolderFactory
            {
                get;
                set;
            }  

            /// <summary>
            /// Do not try to assign to this propety. Exception will be thrown, use <see cref="SpecializedDataTemplate"/> instead.
            /// </summary>
            /// <inheritdoc />
            public DataTemplateDelegate<THolder> DataTemplate
            {
                get => _dataTemplate ?? (_dataTemplate =
                           (item, holder, position) =>
                           {
                               SpecializedDataTemplate((TSpecializedItem) item, (TSpecializedHolder) holder, position);
                           });
                set => throw new InvalidOperationException("Use Specialized factory or base class instead.");
            }

            /// <summary>
            /// Do not try to assign to this propety. Exception will be thrown, use <see cref="SpecializedHolderFactory"/> instead.
            /// </summary>
            /// <inheritdoc />
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

        /// <summary>
        /// Creates new adapter instance with given definitions.
        /// </summary>
        /// <param name="templates">Disctionary of pairs defining how to display given typoe of item.</param>
        /// <param name="items"></param>
        public ObservableRecyclerAdapterWithMultipleViewTypes(
            Dictionary<Type, IItemEntry> templates,
            IList<TItemBase> items)
        {
            _templates = templates;

            DataSource = items;
        }

        public override int GetItemViewType(int position)
        {
            return DataSource[position].GetType().GetHashCode();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DataTemplate = _templates[DataSource[position].GetType()].DataTemplate;
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