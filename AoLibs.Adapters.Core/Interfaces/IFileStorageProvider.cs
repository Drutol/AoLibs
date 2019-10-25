using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    /// <summary>
    /// Provides functionality regarding reading and writing from filesystem.
    /// </summary>
    public interface IFileStorageProvider
    {
        /// <summary>
        /// Reads text from given file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        Task<string> ReadTextAsync(string path);

        /// <summary>
        /// Reads bytes from given file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        Task<byte[]> ReadBytesAsync(string path);

        /// <summary>
        /// Throws exceptions.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="text">Text to save.</param>
        void WriteText(string path, string text);

        /// <summary>
        /// Throws exceptions.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="text">Text to save.</param>
        Task WriteTextAsync(string path, string text);

        /// <summary>
        /// Throws exceptions.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="bytes">Bytes to save.</param>
        void WriteBytes(string path, byte[] bytes);

        /// <summary>
        /// Throws exceptions.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="bytes">Bytes to save.</param>
        Task WriteBytesAsync(string path, byte[] bytes);

        /// <summary>
        /// Removes files at the specified path.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        void RemoveFile(string path);

        /// <summary>
        /// Checking if file on provided <paramref name="path"/> exists.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        bool CheckIfFileExists(string path);
    }
}
