using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    /// <summary>
    /// Interface used to allow resolve proper navigation stack for given page identifier.
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    /// <typeparam name="TPageIdentifier"></typeparam>
    public interface IStackResolver<TPage, in TPageIdentifier> where TPage : class
    {
        /// <summary>
        /// Resolves stack for given <see cref="TPageIdentifier"/>
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        TaggedStack<BackstackEntry<TPage>> ResolveStackForIdentifier(TPageIdentifier identifier);
        /// <summary>
        /// Resolves stack for given <see cref="Enum"/> which should be <see cref="TPageIdentifier"/>
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        TaggedStack<BackstackEntry<TPage>> ResolveStackForTag(Enum tag);
    }
}