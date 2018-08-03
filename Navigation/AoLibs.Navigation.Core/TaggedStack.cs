using System;
using System.Collections.Generic;

namespace AoLibs.Navigation.Core
{
    /// <summary>
    /// Stack with added Tag property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaggedStack<T> : Stack<T>
    {
        public Enum Tag { get; set; }
    }
}
