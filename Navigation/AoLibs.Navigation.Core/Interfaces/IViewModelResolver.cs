namespace AoLibs.Navigation.Core.Interfaces
{
    public interface IViewModelResolver
    {
        TViewModel Resolve<TViewModel>();
    }
}
