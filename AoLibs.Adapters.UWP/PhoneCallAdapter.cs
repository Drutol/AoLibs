
using Windows.ApplicationModel.Calls;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Simple adapter allowing to call given number.
    /// </summary>
    public class PhoneCallAdapter : IPhoneCallAdapter
    {
        public void Call(string telephoneNumber)
        {
            PhoneCallManager.ShowPhoneCallUI(telephoneNumber, string.Empty);
        }
    }
}