// ReSharper disable once CheckNamespace
namespace AoLibs.Utilities.Shared
{
    internal sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers = true;
        public bool Conditional;
    }
}