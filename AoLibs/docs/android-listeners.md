# Android Listeners

Set of various listeners that are accepting delegates to native Android callback classes.

* `DateSetListener : DatePickerDialog.IOnDateSetListener`
* `GenericMotionListener :  View.IOnGenericMotionListener`
* `OnCheckedListener : RadioGroup.IOnCheckedChangeListener`
* `OnClickListener : View.IOnClickListener`
* `OnEditorActionListener : TextView.IOnEditorActionListener`
* `OnItemClickListener<T> : AdapterView.IOnItemClickListener`
    * Expects the item to be wrapped with `JavaObjectWrapper<T>` withing `Tag` property.
* `OnLongClickListener : View.IOnLongClickListener`
* `OnScrollListener : AbsListView.IOnScrollListener`
* `OnTextEnterListener : ITextWatcher`
    * If EditText can be multiline this listner will detect wheter the user presses "Enter" on their keyboard.
* `OnTouchListener : View.IOnTouchListener`
* `ScrollListener : AbsListView.IOnScrollChangeListener`



