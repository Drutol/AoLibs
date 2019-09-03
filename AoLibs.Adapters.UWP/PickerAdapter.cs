using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Class that presents system picker allowing user to choose from given collection of items.
    /// </summary>
    public class PickerAdapter : IPickerAdapter
    {
        public Task<int?> ShowItemsPicker(IEnumerable<string> items, int selectedIndex, string title, string cancelText, string okText)
        {
            throw new NotImplementedException();
        }

        public Task<DateTime?> ShowDatePicker(DateTime startingDate, string okText, string cancelText = null)
        {
            throw new NotImplementedException();
        }
    }
}