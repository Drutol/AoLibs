using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    public class PhotoPickerAdapter : IPhotoPickerAdapter
    {
        private const int TakePhotoRequestId = 12;

        private readonly IContextProvider _contextProvider;
        private readonly IOnActivityResultProvider _activityResultProvider;

        public PhotoPickerAdapter(IContextProvider contextProvider, IOnActivityResultProvider activityResultProvider)
        {
            _contextProvider = contextProvider;
            _activityResultProvider = activityResultProvider;           
        }

        public async Task<byte[]> PickPhoto()
        {
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            _contextProvider.CurrentContext.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"),TakePhotoRequestId);

            var (_, _, data) = await _activityResultProvider.Await();
            
            if (data == null)
                return null;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[1024];
                using (var stream = _contextProvider.CurrentContext.ContentResolver.OpenInputStream(data.Data))
                {
                    int len;
                    while ((len = await stream.ReadAsync(buffer, 0, 1024)) > 0)
                    {
                        await ms.WriteAsync(buffer, 0, len);
                    }
                }

                return ms.ToArray();
            }          
        }
    }
}