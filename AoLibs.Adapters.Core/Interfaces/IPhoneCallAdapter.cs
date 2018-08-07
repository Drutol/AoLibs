namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Simple adapter allowing to call given number.
    /// </summary>
    public interface IPhoneCallAdapter
    {
        /// <summary>
        /// Presents system controls for calling with given number filled in.
        /// </summary>
        /// <param name="telephoneNumber"></param>
        void Call(string telephoneNumber);
    }
}
