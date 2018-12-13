# Recycler View Adapters

Adapters library contains set of adapters that eliminate need for creating custom adapter classes.

Applicable types:

Adapters:

* `ObservableRecyclerAdapter<TItem, THolder>`
* `ObservableRecyclerAdapterWithFooter<TItem, THolder, TFooterHolder>`
* `ObservableRecyclerAdapterWithMultipleViewTypes<TItemBase, THolder>`
* `ObservableRecyclerAdapterWithMultipleViewTypesAndFooter<TItemBase, THolder, TFooterHolder>`

ViewHolder:

* `BindingViewHolderBase<T>`

Utilities:

* `ItemEntry`

* `SpecializedItemEntry<TSpecializedItem, TSpecializedHolder>`

These classes provide an easy way to quickly implement recycler views for various scenarios starting from simple list to elaborate lists with various ViewModel bound elements with different layouts. All of provided adapters are utilizing events of `ObservableCollection<T>`.

The `ObservableRecyclerAdapter<TItem, THolder>` is base class for all other specialized adapters.

## Simple setup

So let's say we want to set-up simple recycler view list. Let's assume we have layout already created with `RecyclerView` named _RecyclerView_

```cs
RecyclerView.SetAdapter(
    adapter: new ObservableRecyclerAdapter<Item, ItemViewHolder>(
        items: ViewModel.Items, 
        dataTemplate: DataTemplate,
        itemTemplate: ItemTemplate,
        holderFactory: HolderFactory));

private void DataTemplate(Item item, ItemViewHolder holder, int position)
{
    holder.SomeTextView.Text = item.SomeTextValue;
}

private View ItemTemplate(int viewType)
{
    return LayoutInflater.Inflate(Resource.Layout.some_item_layout, null);
}

private ItemViewHolder HolderFactory(ViewGroup parent, int viewType, View view)
{
    return new ItemViewHolder(view);
}
```

We have 3 methods being passed to adapter's constructor:

* `DataTemplate`
	* Here we are defining how to display given item in the list, you will receive current item and holder that will have to display it.
* `ItemTemplate`
	* Here you should provide view that will be later bound to view holder.
* `HolderFactory`
	* Here we are instantiating holder for our virtualized items. This parameter is optional if your holder has public parameterless constructor and you don't need custom instantiation.

## View Stretching

Unless specified the views in `RecyclerView` won't stretch horizontally. I've provided simple utility that takes care of it. 

```cs
RecyclerView.SetAdapter(
    adapter: new ObservableRecyclerAdapter<Item, ItemViewHolder>(
        items: ViewModel.Items, 
        dataTemplate: DataTemplate,
        itemTemplate: ItemTemplate,
        holderFactory: HolderFactory) {StretchContentHorizonatally = true});
```

In order to use it you just have to set `StretchContentHorizonatally` or `StretchContentVertically` property to `true`. Item containers will be assigned with proper `RecyclerView.LayoutParams` after inflating.

## Footer

Adapter with footer is very similar to standard adapter:

```cs
RecyclerView.SetAdapter(
    new ObservableRecyclerAdapterWithFooter<Item, ItemViewHolder, FooterHolder>(
            items: ViewModel.Items, 
            dataTemplate: DataTemplate, 
            holderFactory: HolderFactory, 
            itemTemplate: ItemTemplate,
            footerDataTemplate: FooterDataTemplate,
            footerFactory: FooterFactory,
            footerTemplate: FooterTemplate)
        {ApplyLayoutParams = true});

...

private View FooterTemplate(int viewType)
{
    return LayoutInflater.Inflate(Resource.Layout.some_footer, null);
}

private FooterHolder FooterFactory(ViewGroup parent, int viewType, View view)
{
    return new FooterHolder(view);
}

private void FooterDataTemplate(Item item, FooterHolder holder, int position)
{
    holder.ViewModel = ViewModel;
}
```

All you have to do is to add 3 more delegates defining footer. Mind you that `item` argument in `FooterDataTemplate` is `null` as there's no item applicable for that position.

> Note: If your footer holder inherits from `BindingViewHolderBase<TViewModel>` the holder won't automatically update bindings, you have to assign your ViewModel manually to footer.

## Binding ViewHolders

As I've previously mentioned it's possible to easily implement bindings for each list entry.
All you have to do is to make your ViewHolder inherit from `BindingViewHolderBase<TViewModel>`.
For example:

```cs
private class SomeHolder : BindingViewHolderBase<SomeViewModel>
{
    private readonly View _view;

    private TextView _totalPriceLabel;

    public FooterHolder(View view) : base(view)
    {
        _view = view;
    }

    public TextView TotalPriceLabel => _totalPriceLabel ??
                                       (_totalPriceLabel = _view.FindViewById<TextView>(Resource.Id.TotalPriceLabel));

    protected override void SetBindings()
    {
        Bindings.Add(this.SetBinding(() => ViewModel.TotalOrderPrice, () => TotalPriceLabel.Text)
            .ConvertSourceToTarget(arg => arg.ToString("C")));
    }
}
```

This view holder will make you implement `SetBindings` methods which works exactly the same as in Fragment's bindings. ViewModel swapping and bindings detaching will all be handled by the library.

## Adapters with multiple view types

This is where the fun starts. We have this lovely class `ObservableRecyclerAdapterWithMultipleViewTypes<TItemBase, THolder>` which is inheriting from `ObservableRecyclerAdapter<TItemBase,THolder>`.
Notice that there's no longer `TItem` but `TItemBase` that means we will provide list of various elements. You can go for `object` here but I encourage to make your models share same even empty interface for cleanliness sake. 

The constructor has changed too: `ObservableRecyclerAdapterWithMultipleViewTypes(Dictionary<Type, IItemEntry> templates,IList<TItemBase> items)` we will have to provide a definition of view per item.

```cs
RecyclerView.SetAdapter(
new ObservableRecyclerAdapterWithMultipleViewTypes<IFriendListItem,
        RecyclerView.ViewHolder>(
        new Dictionary<Type, ObservableRecyclerAdapterWithMultipleViewTypes<IFriendListItem,
            RecyclerView.ViewHolder>.IItemEntry>
        {
            {
                typeof(FriendListHeaderItem),
                new ObservableRecyclerAdapterWithMultipleViewTypes<IFriendListItem,
                    RecyclerView.ViewHolder>.SpecializedItemEntry<FriendListHeaderItem,
                    SectionViewHolder>
                {
                    SpecializedHolderFactory = HeaderHolderFactory,
                    ItemTemplate = HeaderItemTemplate,
                    SpecializedDataTemplate = HeaderDataTemplate
                }
            },
            {
                typeof(InviteFriendItemViewModel),
                new ObservableRecyclerAdapterWithMultipleViewTypes<IFriendListItem,
                    RecyclerView.ViewHolder>.SpecializedItemEntry<InviteFriendItemViewModel,
                    FriendViewHolder>
                {
                    SpecializedHolderFactory = ItemHolderFactory,
                    ItemTemplate = ItemTemplate,
                    SpecializedDataTemplate = ItemDataTemplate
                }
            }
        }, ViewModel.Friends)
    {ApplyLayoutParams = true});
```

It may look intimidating at first but it's not that bad. Essentially we are providing pairs of `Type` and `IItemEntry` implementations that are found in the library.

`IFriendListItem` is our `TItemBase`
`TViewHolder` will usually be simple `RecyclerView.ViewHolder`

Let's take a closer look on view-type definition.

```cs
{
    typeof(InviteFriendItemViewModel),
    new ObservableRecyclerAdapterWithMultipleViewTypes<IFriendListItem,
        RecyclerView.ViewHolder>.SpecializedItemEntry<InviteFriendItemViewModel,
        FriendViewHolder>
    {
        SpecializedHolderFactory = ItemHolderFactory,
        ItemTemplate = ItemTemplate,
        SpecializedDataTemplate = ItemDataTemplate
    }
}
```

First thing we provide is our concrete type that is contained within the items to display, easy enough.
Now comes the second part, `ObservableRecyclerAdapterWithMultipleViewTypes` has inner classes:

* `SpecializedItemEntry`
* `ItemEntry`
They are used to define the views. `SpecializedItemEntry` is an evolution of `ItemEntry` that handles type casting so we don't have to do it manually each time.

```cs
public class SpecializedItemEntry<TSpecializedItem, TSpecializedHolder> : IItemEntry
    where TSpecializedItem : TItemBase
    where TSpecializedHolder : THolder
```

Instead of getting `TItemBase` and `THolder` passed to our `DataTemplate` we will be getting solid instances of `InviteFriendItemViewModel` and `FriendViewHolder`. If you wish to do the casting manually you can use `ItemEntry`.

> Don't use `HolderFactory` and `DataTemplate` properties in `SpecializedItemEntry` and use `SpecializedHolderFactory` and `SpecializedDataTemplate` properties instead. Exception will be thrown otherwise.

```cs
{
    typeof(RestaurantPageMenuItemViewModel),
    new ObservableRecyclerAdapterWithMultipleViewTypes<
            RestaurantPageViewModel.IRestaurantMenuItem, RecyclerView.ViewHolder>.ItemEntry
        {
            ItemTemplate = MenuItemTemplate,
            DataTemplate = MenuDataTemplate,
            HolderFactory = MenuHolderFactory
        }
}
```

Of course you can use your friend `BindingViewHolderBase<TViewModel>` to achieve bindings per list item too!

## Adapters with multiple view types and footer

The ultimate form, handles just like `ObservableRecyclerAdapterWithMultipleViewTypes` with addition of 3 delegates like in `ObservableRecyclerAdapterWithFooter`. 

```cs
RecyclerView.SetAdapter(
	new ObservableRecyclerAdapterWithMultipleViewTypesAndFooter<ReceiptPresentPageViewModel.IReceiptItem
	    , RecyclerView.ViewHolder, FooterViewHolder>(
	    new Dictionary<Type, ObservableRecyclerAdapterWithMultipleViewTypes<
	        ReceiptPresentPageViewModel.IReceiptItem, RecyclerView.ViewHolder>.IItemEntry>
	    {
	        {
	            typeof(ReceiptHeaderItem),
	            new ObservableRecyclerAdapterWithMultipleViewTypes<
	                    ReceiptPresentPageViewModel.IReceiptItem, RecyclerView.ViewHolder>.
	                SpecializedItemEntry<ReceiptHeaderItem, HeaderViewHolder>
	                {
	                    SpecializedHolderFactory = HeaderHolderFactory,
	                    ItemTemplate = HeaderItemTemplate,
	                    SpecializedDataTemplate = HeaderDataTemplate
	                }
	        },
	        {
	            typeof(ReceiptOrderedDishItemViewModel),
	            new ObservableRecyclerAdapterWithMultipleViewTypes<
	                    ReceiptPresentPageViewModel.IReceiptItem, RecyclerView.ViewHolder>.
	                SpecializedItemEntry<ReceiptOrderedDishItemViewModel, OrderedDishViewHolder>
	                {
	                    SpecializedHolderFactory = OrderedDishHolderFactory,
	                    ItemTemplate = OrderedDishItemTemplate,
	                    SpecializedDataTemplate = OrderedDishDataTemplate
	                }
	        },
	        {
	            typeof(ReceiptOrderSectionFooterItem),
	            new ObservableRecyclerAdapterWithMultipleViewTypes<
	                    ReceiptPresentPageViewModel.IReceiptItem, RecyclerView.ViewHolder>.
	                SpecializedItemEntry<ReceiptOrderSectionFooterItem, SectionFooterViewHolder>
	                {
	                    SpecializedHolderFactory = SectionFooterHolderFactory,
	                    ItemTemplate = SectionFooterItemTemplate,
	                    SpecializedDataTemplate = SectionFooterDataTemplate
	                }
	        }
	    }, 
	    ViewModel.CurrentReceipt, 
	    FooterDataTemplate, 
	    FooterFactory, 
	    FooterTemplate)
	{
	    ApplyLayoutParams = true
	});
```