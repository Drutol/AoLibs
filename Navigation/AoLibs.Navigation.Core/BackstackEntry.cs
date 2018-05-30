using System;

namespace AoLibs.Navigation.Core
{
    public class BackstackEntry<TPage> where TPage : class
    {
        public TPage Page { get; set; }
        public Action OnBackNavigation { get; set; }
    }
}
