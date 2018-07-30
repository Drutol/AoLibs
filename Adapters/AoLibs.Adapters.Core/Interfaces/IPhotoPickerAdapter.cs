using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IPhotoPickerAdapter
    {
        Task<byte[]> PickPhoto(string pickerTitle);
    }
}
