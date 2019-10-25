using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Adapters.Core.Interfaces;

namespace AoLibs.Adapters.iOS
{
    public class FileStorageProvider : IFileStorageProvider
    {
        private string _libraryPath;

        private string ResolvePath(string path)
        {
            var finalPath = _libraryPath;
            foreach (var pathPiece in path.Split('/').ToArray())
                finalPath = Path.Combine(finalPath, pathPiece);

            return finalPath;
        }

        public FileStorageProvider()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            _libraryPath = Path.Combine(documentsPath, "..", "Library", "Caches"); // Library folder
        }

        public async Task<string> ReadTextAsync(string path)
        {
            try
            {
                var text = new StringBuilder();
                using (var fs = File.OpenRead(ResolvePath(path)))
                {
                    using (var reader = new StreamReader(fs))
                    {
                        text.Append(await reader.ReadLineAsync());
                    }
                }

                return text.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<byte[]> ReadBytesAsync(string path)
        {
            try
            {
                using (var streamReader = new StreamReader(ResolvePath(path)))
                {
                    using (var memstream = new MemoryStream())
                    {
                        await streamReader.BaseStream.CopyToAsync(memstream);
                        return memstream.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void WriteText(string path, string text)
        {
            path = EnsureFileCreated(path);

            using (var fs = File.Create(path))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(text);
                }
            }
        }

        public async Task WriteTextAsync(string path, string text)
        {
            path = EnsureFileCreated(path);

            using (var fs = File.Create(path))
            {
                using (var writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(text);
                }
            }
        }

        public void WriteBytes(string path, byte[] bytes)
        {
            path = EnsureFileCreated(path);

            using (var fs = File.Create(path))
            {
                fs.Write(bytes,0,bytes.Length);
            }
        }

        public async Task WriteBytesAsync(string path, byte[] bytes)
        {
            path = EnsureFileCreated(path);

            using (var fs = File.Create(path))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public void RemoveFile(string path)
        {
            File.Delete(ResolvePath(path));
        }

        public bool CheckIfFileExists(string path)
        {
            var dirPath = path;
            if (dirPath.Contains('/'))
            {
                dirPath = path.Substring(0, path.LastIndexOf('/'));
                dirPath = ResolvePath(dirPath);
            }

            return File.Exists(dirPath);
        }

        private string EnsureFileCreated(string path)
        {
            var dirPath = path;
            if (dirPath.Contains('/'))
            {
                dirPath = path.Substring(0,path.LastIndexOf('/'));
                dirPath = ResolvePath(dirPath);
                if (!File.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            path = ResolvePath(path);

            return path;
        }
    }
}