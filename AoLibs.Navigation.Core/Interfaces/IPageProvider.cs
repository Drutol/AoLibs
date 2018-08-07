using System;

namespace AoLibs.Navigation.Core.Interfaces
{
    public interface IPageProvider<out T> where T : INavigationPage
    {  
        Type PageType { get; }
        T Page { get; }
        object PageIdentifier { get; set; }
        void ForceReinstantination();
    }
}