using System;
using Android.Text;
using Java.Lang;

namespace AoLibs.Utilities.Android.Listeners
{
    /// <summary>
    /// Listens if user has pressed enter during text edition.
    /// </summary>
    public class OnTextEnterListener : Java.Lang.Object, ITextWatcher
    {
        private readonly Action _onEnter;

        public OnTextEnterListener(Action onEnter)
        {
            _onEnter = onEnter;
        }

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
 
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (!string.IsNullOrWhiteSpace(s.ToString()) && s.ToString().EndsWith("\n"))
                _onEnter.Invoke();
        }
    }
}