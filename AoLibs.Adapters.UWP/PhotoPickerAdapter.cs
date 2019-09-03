using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Class that allows to obtain image that user is asked to choose.
    /// </summary>
    public class PhotoPickerAdapter : IPhotoPickerAdapter
    {
        public Task<byte[]> PickPhoto(string pickerTitle)
        {
            throw new System.NotImplementedException();
        }
    }
}