namespace AoLibs.Navigation.Core
{
    public enum NavigationBackstackOption
    {
        /// <summary>
        ///     Adds page to backstack as usual.
        /// </summary>
        AddToBackstack,

        /// <summary>
        ///     Clears backstack and sets given page as root.
        /// </summary>
        SetAsRootPage,

        /// <summary>
        ///     Backstack will be cleared until page of given type is found. Upon failure this page will be root of naviagtion
        ///     stack.
        /// </summary>
        ClearBackstackToFirstOccurence,

        /// <summary>
        ///     Page won't be pushed on backstack.
        /// </summary>
        NoBackstack,

        /// <summary>
        ///     Ensures that page will not preserve its saved state.
        /// </summary>
        ForceNewPageInstance
    }
}