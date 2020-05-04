using System;
using System.IO;
using System.Linq;
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
        private const string Extension = ".data";

        private readonly StorageFolder _localFolder;

        public FileStorageProvider()
        {
            _localFolder = ApplicationData.Current.LocalFolder;
        }


        public async Task<string> ReadTextAsync(string path)
        {
            try
            {
                return await FileIO.ReadTextAsync(await GetFile(path, false));
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
                var buffer = await FileIO.ReadBufferAsync(await GetFile(path, false));

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
            await FileIO.WriteTextAsync(await GetFile(path, true), text);
        }

        public async void WriteBytes(string path, byte[] bytes)
        {
            await WriteBytesAsync(path, bytes);
        }

        public async Task WriteBytesAsync(string path, byte[] bytes)
        {
            await FileIO.WriteBufferAsync(await GetFile(path, true), bytes.AsBuffer());
        }

        public async void RemoveFile(string path)
        {
            await (await GetFile(path, false)).DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        public async Task<Stream> OpenFile(string path, FileMode mode)
        {
            return (await (await GetFile(path, false)).OpenAsync(FileAccessMode.ReadWrite)).AsStream();
        }

        public async Task<Stream> CreateFile(string path)
        {
            return (await (await GetFile(path, true)).OpenAsync(FileAccessMode.ReadWrite)).AsStream();
        }

        public async Task<string> ResolveLocalPath(string path)
        {
            return (await GetFile(path, false)).Path;
        }

        private async Task<IStorageFile> GetFile(string path, bool create)
        {
            path += Extension;

            var absolutePath = Path.Combine(_localFolder.Path, path);
            var fileInfo = new FileInfo(absolutePath);
            if (!fileInfo.Exists)
            {
                if (!create)
                    return null;

                var folderTokens = path.Split('/');
                var currentFolderPath = _localFolder.Path;
                var currentFolder = _localFolder;
                foreach (var folderPath in folderTokens.Take(folderTokens.Length - 1))
                {
                    currentFolderPath = Path.Combine(currentFolderPath, folderPath);
                    currentFolder = await currentFolder.CreateFolderAsync(folderPath, CreationCollisionOption.OpenIfExists);
                }
                return await currentFolder.CreateFileAsync(Path.GetFileName(path));
            }
            else
            {
                return await StorageFile.GetFileFromPathAsync(absolutePath.Replace('/', Path.DirectorySeparatorChar));
            }
        }
    }
}