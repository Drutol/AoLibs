using System;

namespace AoLibs.Navigation.Core
{
    /// <summary>
    /// Represents an item in naviagtrion backstack. It can be either <see cref="Action"/> or <see cref="TPage"/>
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    public class BackstackEntry<TPage> where TPage : class
    {
        public TPage Page { get; set; }
        public Action OnBackNavigation { get; set; }
    }
}
