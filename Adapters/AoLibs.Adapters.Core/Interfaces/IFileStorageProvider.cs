using System.Threading.Tasks;

namespace AoLibs.Adapters.Core.Interfaces
{
    public interface IFileStorageProvider
    {
        Task<string> ReadTextAsync(string path);
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
        void RemoveFile(string path);
    }
}
