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
        Task<string> ReadTextAsync(string path);
        /// <summary>
        /// Reads bytes from given file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<byte[]> ReadBytesAsync(string path);
        /// <summary>
        /// Throws exceptions.
        /// </summary>
        void WriteText(string path, string text);
        /// <summary>
        /// Throws exceptions.
        /// </summary>
        Task WriteTextAsync(string path, string text);
        /// <summary>
        /// Throws exceptions.
        /// </summary>
        void WriteBytes(string path, byte[] bytes);
        /// <summary>
        /// Throws exceptions.
        /// </summary>
        Task WriteBytesAsync(string path, byte[] bytes);
        /// <summary>
        /// Removes files at the specified path.
        /// </summary>
        void RemoveFile(string path);
    }
}
