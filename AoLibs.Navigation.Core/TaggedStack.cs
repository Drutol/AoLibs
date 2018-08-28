using System;
using System.Collections.Generic;

namespace AoLibs.Navigation.Core
{
    /// <summary>
    /// Stack with added Tag property.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    public class TaggedStack<T> : Stack<T>
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        public Enum Tag { get; set; }
    }
}
