// ReSharper disable once CheckNamespace
#pragma warning disable SA1401 // Fields must be private
namespace AoLibs.Adapters.Core
{
    internal sealed class PreserveAttribute : System.Attribute
    {
        public bool AllMembers = true;
        public bool Conditional;
    }
}
#pragma warning restore SA1401 // Fields must be private