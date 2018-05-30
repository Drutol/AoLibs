using Android.Content;
using Android.Runtime;
using AoLibs.Adapters.Android.Interfaces;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.Android
{
    [Preserve(AllMembers = true)]
    public class PhoneCallAdapter : IPhoneCallAdapter
    {
        private readonly IContextProvider _contextProvider;

        public PhoneCallAdapter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public void Call(string telephoneNumber)
        {
            var uri = global::Android.Net.Uri.Parse($"tel:{telephoneNumber}");
            var intent = new Intent(Intent.ActionDial, uri);
            _contextProvider.CurrentContext.StartActivity(intent);
        }
    }
}