using System;
using System.Collections.Generic;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace AoLibs.Adapters.Android.Recycler
{
    /// <summary>
    /// Utility class that alows to fluently build an adapter for recycler including multi view type layouts.
    /// Variant with default recycler holder.
    /// </summary>
    /// <typeparam name="TItem">Base type of the items displayed in recycler.</typeparam>
    public class RecyclerViewAdapterBuilder<TItem> : RecyclerViewAdapterBuilder<TItem, RecyclerView.ViewHolder>
    {
    }

    /// <summary>
    /// Utility class that alows to fluently build an adapter for recycler including multi view type layouts.
    /// </summary>
    /// <typeparam name="TItem">Base type of the items displayed in recycler.</typeparam>
    /// <typeparam name="THolder">Base type of the holder class.</typeparam>
    public class RecyclerViewAdapterBuilder<TItem, THolder>
        : RecyclerViewAdapterBuilder<TItem, THolder>.IAdapterBuilder,
            RecyclerViewAdapterBuilder<TItem, THolder>.IMultipleViewsAdapterBuilder
        where THolder : RecyclerView.ViewHolder
    {
        private IList<TItem> _collection;
        private ObservableRecyclerAdapter<TItem, THolder>.DataTemplateDelegate<THolder> _dataTemplate;
        private Dictionary<Type, ObservableRecyclerAdapterWithMultipleViewTypes<TItem, THolder>.IItemEntry> _groups;
        private ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<THolder> _holderTemplate;
        private LayoutInflater _inflater;
        private bool _isMultipleViews;
        private ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate _itemTemplate;
        private int _layoutResourceId;
        private bool _stretchHorizontally;
        private bool _stretchVertically;

        public RecyclerView.Adapter Build()
        {
            if (_isMultipleViews)
            {
                return new ObservableRecyclerAdapterWithMultipleViewTypes<TItem, THolder>(_groups, _collection)
                {
                    StretchContentHorizonatally = _stretchHorizontally,
                    StretchContentVertically = _stretchVertically
                };
            }

            if (_inflater == null)
            {
                return new ObservableRecyclerAdapter<TItem, THolder>(
                    _collection, 
                    _dataTemplate,
                    _itemTemplate,
                    _holderTemplate);
            }

            return new ObservableRecyclerAdapter<TItem, THolder>(
                    _collection,
                    _dataTemplate,
                    _inflater,
                    _layoutResourceId,
                    _holderTemplate)
            {
                StretchContentHorizonatally = _stretchHorizontally, 
                StretchContentVertically = _stretchVertically
            };
        }

        public IAdapterBuilder WithItems(IList<TItem> collection)
        {
            _collection = collection;
            return this;
        }

        public IAdapterBuilder WithContentStretching(bool horizontal = true, bool vertical = false)
        {
            _stretchHorizontally = horizontal;
            _stretchVertically = vertical;
            return this;
        }

        public IAdapterBuilder WithItemTemplate(ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate template)
        {
            _itemTemplate = template;
            return this;
        }

        public IAdapterBuilder WithDataTemplate(
            ObservableRecyclerAdapter<TItem, THolder>.DataTemplateDelegate<THolder> template)
        {
            _dataTemplate = template;
            return this;
        }

        public IAdapterBuilder WithHolderTemplate(
            ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<THolder> template)
        {
            _holderTemplate = template;
            return this;
        }

        public IAdapterBuilder WithResourceId(LayoutInflater inflater, int layoutResourceId)
        {
            _inflater = inflater;
            _layoutResourceId = layoutResourceId;
            return this;
        }

        public IMultipleViewsAdapterBuilder WithMultipleViews()
        {
            _isMultipleViews = true;
            _groups = new Dictionary<Type, ObservableRecyclerAdapterWithMultipleViewTypes<TItem, THolder>.IItemEntry>();
            return this;
        }

        public IMultipleViewsAdapterBuilder WithGroup<TGroupItem, TGroupHolder>(
            Action<IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder>> options)
            where TGroupItem : TItem
            where TGroupHolder : THolder
        {
            var builder = new MultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder>();
            options(builder);
            _groups.Add(typeof(TGroupItem), builder.Build());
            return this;
        }

        public interface IBaseAdapterBuilder
        {
            RecyclerView.Adapter Build();
        }

        public interface IAdapterBuilder : IBaseAdapterBuilder
        {
            IAdapterBuilder WithItemTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate template);

            IAdapterBuilder WithDataTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.DataTemplateDelegate<THolder> template);

            IAdapterBuilder WithHolderTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<THolder> template);

            IAdapterBuilder WithResourceId(LayoutInflater inflater, int layoutResourceId);
            IAdapterBuilder WithItems(IList<TItem> collection);
            IAdapterBuilder WithContentStretching(bool horizontal = true, bool vertical = false);

            IMultipleViewsAdapterBuilder WithMultipleViews();
        }

        public interface IMultipleViewsAdapterBuilder : IBaseAdapterBuilder
        {
            IMultipleViewsAdapterBuilder WithGroup<TGroupItem, TGroupHolder>(
                Action<IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder>> options)
                where TGroupHolder : THolder 
                where TGroupItem : TItem;
        }

        public interface IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder>
            where TGroupHolder : THolder
            where TGroupItem : TItem
        {
            IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithDataTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.SpecializedDataTemplateDelegate<TGroupHolder, TGroupItem>
                    template);

            IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithItemTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate template);

            IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithHolderTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<TGroupHolder> template);

            IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithResourceId(
                LayoutInflater inflater,
                int layoutResourceId);
        }

        private class
            MultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> : IMultipleViewsAdapterGroupBuilder<TGroupItem,
                TGroupHolder> 
            where TGroupItem : TItem 
            where TGroupHolder : THolder
        {
            private ObservableRecyclerAdapter<TItem, THolder>.SpecializedDataTemplateDelegate<TGroupHolder, TGroupItem>
                _dataTemplate;

            private ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<TGroupHolder> _holderTemplate;
            private ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate _itemTemplate;
            private LayoutInflater _layoutInflater;
            private int _resourceId;

            public IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithDataTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.SpecializedDataTemplateDelegate<TGroupHolder, TGroupItem>
                    template)
            {
                _dataTemplate = template;
                return this;
            }

            public IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithHolderTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.HolderFactoryDelegate<TGroupHolder> template)
            {
                _holderTemplate = template;
                return this;
            }

            public IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithItemTemplate(
                ObservableRecyclerAdapter<TItem, THolder>.ItemTemplateDelegate template)
            {
                _itemTemplate = template;
                return this;
            }

            public IMultipleViewsAdapterGroupBuilder<TGroupItem, TGroupHolder> WithResourceId(
                LayoutInflater inflater,
                int layoutResourceId)
            {
                _layoutInflater = inflater;
                _resourceId = layoutResourceId;
                return this;
            }

            public ObservableRecyclerAdapterWithMultipleViewTypes<TItem, THolder>.IItemEntry Build()
            {
                return new ObservableRecyclerAdapterWithMultipleViewTypes<TItem, THolder>.SpecializedItemEntry<
                    TGroupItem, TGroupHolder>
                {
                    SpecializedDataTemplate = _dataTemplate,
                    SpecializedHolderFactory = _holderTemplate ?? ((parent, type, view) =>
                                                   (TGroupHolder)Activator.CreateInstance(typeof(TGroupHolder), view)),
                    ItemTemplate = _itemTemplate ?? (type => _layoutInflater.Inflate(_resourceId, null))
                };
            }
        }
    }
}
