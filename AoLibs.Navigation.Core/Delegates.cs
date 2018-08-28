namespace AoLibs.Navigation.Core
{ 
    /// <summary>
    /// Delegate allowing to intercept navigation before actual commit.
    /// </summary>
    /// <typeparam name="TPageIdentifier">Page enum type.</typeparam>
    /// <param name="targetPage">Page that is the target of navigation.</param>
    public delegate TPageIdentifier NaviagtionInterceptor<TPageIdentifier>(TPageIdentifier targetPage);
}
