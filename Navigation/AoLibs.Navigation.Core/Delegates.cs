namespace AoLibs.Navigation.Core
{ 
    /// <summary>
    /// Delegate allowing to intercept navigation before actual commit.
    /// </summary>
    /// <typeparam name="TPageIdentifier"></typeparam>
    /// <param name="targetPage"></param>
    /// <returns></returns>
    public delegate TPageIdentifier NaviagtionInterceptor<TPageIdentifier>(TPageIdentifier targetPage);
}
