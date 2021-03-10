using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    /// <summary>
    /// Interface used to allow resolve proper navigation stack for given page identifier.
    /// </summary>
    /// <typeparam name="TPage">The page type used on the target platofrm.</typeparam>
    /// <typeparam name="TPageIdentifier">Enum defining the pages.</typeparam>
    public interface IStackResolver<TPage, in TPageIdentifier> 
        where TPage : class
    {
        /// <summary>
        /// Resolves stack for given <see cref="TPageIdentifier"/>.
        /// </summary>
        /// <param name="identifier">Page related to desired stack.</param>
        /// <returns>Found stack.</returns>
        TaggedStack<BackstackEntry<TPage>> ResolveStackForIdentifier(TPageIdentifier identifier);
        
        /// <summary>
        /// Resolves stack for given <see cref="Enum"/> which should be <see cref="TPageIdentifier"/>.
        /// </summary>
        /// <param name="tag">Tag associated with wanted stack.</param>
        /// <returns>Found stack.</returns>
        TaggedStack<BackstackEntry<TPage>> ResolveStackForTag(Enum tag);
    }
}