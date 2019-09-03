using System;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides functionality regarding reading and writing from filesystem.
    /// </summary>
    public class FileStorageProvider : IFileStorageProvider
    {
        public Task<string> ReadTextAsync(string path)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReadBytesAsync(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteText(string path, string text)
        {
            throw new NotImplementedException();
        }

        public Task WriteTextAsync(string path, string text)
        {
            throw new NotImplementedException();
        }

        public void WriteBytes(string path, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public Task WriteBytesAsync(string path, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}