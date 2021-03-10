using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable SA1305 // Field names should not use Hungarian notation
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
        /// <param name="selectedIndex">Initally selected item index. -1 to select none.</param>
        /// <param name="title">Title of the picker.</param>
        /// <param name="cancelText">Text on cancel button.</param>
        /// <param name="okText">Text on ok button.</param>
        /// <param name="dialogStyle">Additional parameter for dialog customization.</param>
        /// <returns>Returns index of selected item, if cancelled returens null.</returns>
        Task<int?> ShowItemsPicker(
            IEnumerable<string> items,
            int selectedIndex,
            string title,
            string cancelText,
            string okText,
            INativeDialogStyle dialogStyle = null);

        /// <summary>
        /// Presents user with date picker.
        /// </summary>
        /// <param name="startingDate">Initialy set date.</param>
        /// <param name="okText">Text on ok button.</param>
        /// <param name="cancelText">Text on cancel button.</param>
        /// <returns>Returns selected date or null if cancelled.</returns>
        Task<DateTime?> ShowDatePicker(
            DateTime startingDate,
            string okText,
            string cancelText = null);
    }
}
