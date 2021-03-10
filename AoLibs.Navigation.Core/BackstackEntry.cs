using System;

namespace AoLibs.Navigation.Core
{
    /// <summary>
    /// Represents an item in navigation backstack. It can be either <see cref="Action"/> or <see cref="TPage"/>.
    /// </summary>
    /// <typeparam name="TPage">Type of page on target platform.</typeparam>
    public class BackstackEntry<TPage> 
        where TPage : class
    {
        public TPage Page { get; set; }
        public Action OnBackNavigation { get; set; }
    }
}
