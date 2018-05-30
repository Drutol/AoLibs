using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IPickerAdapter
    {
        /// <summary>
        /// Returns null when user didn't select anything.
        /// </summary>
        Task<int?> ShowItemsPicker(IEnumerable<string> items, int selectedIndex, string title, string cancelText,
            string okText);

        /// <summary>
        /// Returns null when user didn't select anything.
        /// </summary>
        Task<DateTime?> ShowDatePicker(DateTime startingDate, string okText, string cancelText = null);
    }
}
