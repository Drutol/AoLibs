// ReSharper disable once CheckNamespace
namespace AoLibs.Adapters.Core
{
    internal sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers = true;
        public bool Conditional;
    }
}
