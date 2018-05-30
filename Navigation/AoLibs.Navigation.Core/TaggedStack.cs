using System;
using System.Collections.Generic;

namespace AoLibs.Navigation.Core
{
    public class TaggedStack<T> : Stack<T>
    {
        public Enum Tag { get; set; }
    }
}
