namespace AoLibs.Adapters.iOS
{
    //public class PhotoPickerAdapter : IPhotoPickerAdapter
    //{
    //    public async Task<byte[]> PickPhoto()
    //    {
    //        switch (PHPhotoLibrary.AuthorizationStatus)
    //        {
    //            case PHAuthorizationStatus.NotDetermined:
    //                var result = await PHPhotoLibrary.RequestAuthorizationAsync();
    //                if (result == PHAuthorizationStatus.Authorized)
    //                    return await ShowPicker();

    //                return null;
    //            case PHAuthorizationStatus.Authorized when CrossMedia.Current.IsPickPhotoSupported:
    //                return await ShowPicker();
    //            default:
    //                //TODO: MessageBox
    //                return null;
    //        }
    //    }

    //    private static async Task<byte[]> ShowPicker()
    //    {
    //        using (var media = await CrossMedia.Current.PickPhotoAsync())
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            if (media == null)
    //                return null;

    //            media.GetStream().CopyTo(memoryStream);
    //            return memoryStream.ToArray(); 
    //        }
    //    }
    //}
}