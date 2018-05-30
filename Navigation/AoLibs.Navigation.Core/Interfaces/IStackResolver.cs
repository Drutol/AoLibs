using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface IStackResolver<TPage, in TPageIdentifier> where TPage : class
    {
        TaggedStack<BackstackEntry<TPage>> ResolveStackForIdentifier(TPageIdentifier identifier);
        TaggedStack<BackstackEntry<TPage>> ResolveStackForTag(Enum tag);
    }
}