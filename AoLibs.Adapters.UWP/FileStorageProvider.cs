using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.UWP
{
    /// <summary>
    /// Provides functionality regarding reading and writing from filesystem.
    /// </summary>
    public class FileStorageProvider : IFileStorageProvider
    {
        private StorageFolder _localFolder;

        public FileStorageProvider()
        {
            _localFolder = ApplicationData.Current.LocalFolder;
        }


        public async Task<string> ReadTextAsync(string path)
        {
            try
            {
                return await FileIO.ReadTextAsync(await GetFile(path));
            }
            catch
            {
                return null;
            }

        }

        public async Task<byte[]> ReadBytesAsync(string path)
        {
            try
            {
                var buffer = await FileIO.ReadBufferAsync(await GetFile(path));

                var bytes = new byte[buffer.Length];
                buffer.CopyTo(bytes);
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        public async void WriteText(string path, string text)
        {
            await WriteTextAsync(path, text);
        }

        public async Task WriteTextAsync(string path, string text)
        {
            await FileIO.WriteTextAsync(await GetFile(path), text);
        }

        public async void WriteBytes(string path, byte[] bytes)
        {
            await WriteBytesAsync(path, bytes);
        }

        public async Task WriteBytesAsync(string path, byte[] bytes)
        {
            await FileIO.WriteBufferAsync(await GetFile(path), bytes.AsBuffer());
        }

        public async void RemoveFile(string path)
        {
            await (await GetFile(path)).DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        private async Task<IStorageFile> GetFile(string path)
        {
            return await StorageFile.GetFileFromPathAsync(Path.Combine(_localFolder.Path, path));
        }
    }
}