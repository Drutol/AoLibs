using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace AoLibs.Adapters.Android.Recycler
{
    public class
        ObservableRecyclerAdapterWithMultipleViewTypesAndFooter<TItemBase, THolder, TFooterHolder> :
            ObservableRecyclerAdapterWithMultipleViewTypes<TItemBase, THolder> where THolder : RecyclerView.ViewHolder
        where TFooterHolder : RecyclerView.ViewHolder
    {
        private const int FooterItem = 1;

        private readonly DataTemplateDelegate<TFooterHolder> _footerDataTemplate;
        private readonly HolderFactoryDelegate<TFooterHolder> _footerFactory;
        private readonly ItemTemplateDelegate _footerTemplate;

        public ObservableRecyclerAdapterWithMultipleViewTypesAndFooter(Dictionary<Type, IItemEntry> templates,
            IList<TItemBase> items, DataTemplateDelegate<TFooterHolder> footerDataTemplate,
            HolderFactoryDelegate<TFooterHolder> footerFactory, ItemTemplateDelegate footerTemplate) : base(templates,
            items)
        {
            _footerDataTemplate = footerDataTemplate;
            _footerFactory = footerFactory;
            _footerTemplate = footerTemplate;
        }

        public override int GetItemViewType(int position)
        {
            if (IsFooter(position))
                return FooterItem;
            return base.GetItemViewType(position);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (IsFooter(position))
                _footerDataTemplate(default, holder as TFooterHolder, position);
            else
                base.OnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == FooterItem)
            {
                var view = _footerTemplate(viewType);
                if (ApplyLayoutParams)
                    view.LayoutParameters = new RecyclerView.LayoutParams(-1, -2);
                return _footerFactory(parent, viewType, view);
            }

            return base.OnCreateViewHolder(parent, viewType);
        }

        public override int ItemCount => base.ItemCount + 1;

        private bool IsFooter(int pos) => pos == ItemCount - 1;
    }
}