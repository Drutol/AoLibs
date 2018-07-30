using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Class that presents system picker allowing user to choose from given collection of items.
    /// </summary>
    public interface IPickerAdapter
    {
        /// <summary>
        /// Allows user to pick one from given selection of items.
        /// </summary>
        /// <param name="items">Collection of choices.</param>
        /// <param name="selectedIndex">Initally selected item index.</param>
        /// <param name="title">Title of the picker.</param>
        /// <param name="cancelText">Text on cancel button.</param>
        /// <param name="okText">Text on ok button.</param>
        /// <returns>Returns index of selected item, if cancelled returens null.</returns>
        Task<int?> ShowItemsPicker(IEnumerable<string> items, int selectedIndex, string title, string cancelText,
            string okText);

        /// <summary>
        /// Presents user with date picker.
        /// </summary>
        /// <param name="startingDate">Initialy set date.</param>
        /// <param name="cancelText">Text on cancel button.</param>
        /// <param name="okText">Text on ok button.</param>
        /// <returns>Returns selected date or null if cancelled.</returns>
        Task<DateTime?> ShowDatePicker(DateTime startingDate, string okText, string cancelText = null);
    }
}
