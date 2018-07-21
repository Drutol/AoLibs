using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace AoLibs.Adapters.Android.Recycler
{
    /// <summary>
    /// Recycler adapter that work the same way as <see cref="ObservableRecyclerAdapter{TItem,THolder}"/> but with addition of footer.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="THolder"></typeparam>
    /// <typeparam name="TFooterHolder"></typeparam>
    public class
        ObservableRecyclerAdapterWithFooter<TItem, THolder, TFooterHolder> 
        : ObservableRecyclerAdapter<TItem, THolder>
        where THolder : RecyclerView.ViewHolder 
        where TFooterHolder : RecyclerView.ViewHolder
    {
        private const int NormalItem = 0;
        private const int FooterItem = 1;

        private readonly DataTemplateDelegate<TFooterHolder> _footerDataTemplate;
        private readonly HolderFactoryDelegate<TFooterHolder> _footerFactory;
        private readonly ItemTemplateDelegate _footerTemplate;

        public ObservableRecyclerAdapterWithFooter(IList<TItem> items, DataTemplateDelegate<THolder> dataTemplate,
            HolderFactoryDelegate<THolder> holderFactory, ItemTemplateDelegate itemTemplate,
            DataTemplateDelegate<TFooterHolder> footerDataTemplate,
            HolderFactoryDelegate<TFooterHolder> footerFactory, ItemTemplateDelegate footerTemplate) : base(items,
            dataTemplate, itemTemplate, holderFactory)
        {
            _footerDataTemplate = footerDataTemplate;
            _footerFactory = footerFactory;
            _footerTemplate = footerTemplate;
        }

        public override int GetItemViewType(int position)
        {
            if (IsFooter(position))
                return FooterItem;
            return NormalItem;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (IsFooter(position))
            {
                _footerDataTemplate(default, holder as TFooterHolder, position);
            }
            else
                base.OnBindViewHolder(holder,position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == FooterItem)
            {
                var view = _footerTemplate(viewType);
                view.LayoutParameters = GetLayoutParameters(view);
                return _footerFactory(parent, viewType, view);
            }
            return base.OnCreateViewHolder(parent, viewType);
        }

        public override int ItemCount => base.ItemCount + 1;

        private bool IsFooter(int pos) => pos == ItemCount - 1;
    }
}